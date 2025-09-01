namespace Quiz.AspNetUI.Models;

public class HealthModel
{
    private HealthModel()
    {
    }

    public bool IsUrlOk { get; private set; }
    public bool IsApiHealthy { get; private set; }

    public static async Task<HealthModel> CreateAsync(string? apiBase, string? apiQuestions, string? apiHealth)
    {
        var model = new HealthModel();

        if (string.IsNullOrWhiteSpace(apiBase) ||
            string.IsNullOrWhiteSpace(apiQuestions) ||
            string.IsNullOrWhiteSpace(apiHealth))
        {
            model.IsUrlOk = false;
            model.IsApiHealthy = false;
            return model;
        }

        model.IsUrlOk = Uri.IsWellFormedUriString(apiHealth, UriKind.Absolute);

        if (model.IsUrlOk)
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(apiHealth);
                model.IsApiHealthy = response.IsSuccessStatusCode;
            }
            catch
            {
                model.IsApiHealthy = false;
            }

        return model;
    }
}