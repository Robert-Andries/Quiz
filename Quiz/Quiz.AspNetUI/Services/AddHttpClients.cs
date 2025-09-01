namespace Quiz.AspNetUI.Services;

internal static class AddHttpClients
{
    internal static void AddQuizHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("apiBase",
            opts => opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Api:BaseUrl")!));
        builder.Services.AddHttpClient("api",
            opts => opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Api:QuestionsEndpoint")!));
        builder.Services.AddHttpClient("apiHealth",
            opts => opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Api:HealthEndpoint")!));
    }
}