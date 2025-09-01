using System.Net.Http;

namespace Quiz.WPFUI.Utilities;

public class ApiUtilities
{
    public static async Task<bool> IsApiIsRunning(HttpClient httpClient)
    {
        try
        {
            var response = await httpClient.GetAsync("health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}