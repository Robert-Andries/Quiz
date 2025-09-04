using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Quiz.DomainLayer.Entities;

namespace Quiz.WPFUI.Utilities;

public static class Extensions
{
    /// <summary>
    /// Fetches statistics for the given quiz using the provided HttpClient and populates the Statistics object.
    /// </summary>
    /// <param name="statistics">The destination object for the statistics</param>
    /// <param name="client">HttpClient who has the base address set to the api endpoint</param>
    /// <param name="quiz">Source for calculating statistics</param>
    /// <returns>Bool indicating success</returns>
    /// <exception cref="InvalidOperationException">Base adress of client is null</exception>
    public static async Task<bool> GetStatistics(this Statistics statistics, HttpClient client,
        Quiz.DomainLayer.Entities.Quiz quiz)
    {
        if (client.BaseAddress == null)
            throw new InvalidOperationException("Client base address is not set.");
        try
        {
            var jsonText = JsonConvert.SerializeObject(quiz);
            using var response =
                await client.PostAsync("", new StringContent(jsonText, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JsonConvert.PopulateObject(content, statistics);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}