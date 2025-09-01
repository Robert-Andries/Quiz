using Asp.Versioning;
using AspNetCoreRateLimit;
using Quiz.WebApi.HealthChecks;

namespace Quiz.WebApi.StartupConfig;

public static class QuizServices
{
    public static void AddQuizVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opts =>
        {
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.DefaultApiVersion = new ApiVersion(1, 0);
            opts.ReportApiVersions = true;
        });
    }

    public static void AddQuizRateLimiting(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddInMemoryRateLimiting();
    }

    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck<AppSettingsFile>("appsettings_check");
    }
}