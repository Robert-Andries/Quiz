using System.Net.Http;

namespace Quiz.WPFUI.Utilities;

public class ApiUtilities
{
    /// <summary>
    /// Checks if the API is running by sending a GET request to the health endpoint.
    /// </summary>
    /// <returns>Bool indicating if is running or not</returns>
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