using Microsoft.Extensions.Diagnostics.HealthChecks;
using Quiz.Shared.Interfaces;

namespace Quiz.WebApi.HealthChecks;

public class AppSettingsFile : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly IQuizSettings _quizSettings;

    public AppSettingsFile(IQuizSettings quizSettings, IConfiguration configuration)
    {
        _quizSettings = quizSettings;
        _configuration = configuration;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        if (_quizSettings.NumberOfQuestions <= 0)
            throw new Exception("NumberOfQuestions must be greater than 0!");

        if (_quizSettings.MaxNumberOfWrongAnswers < 0)
            throw new Exception("MaxNumberOfWrongAnswers cannot be negative!");

        if (_quizSettings.MaxNumberOfWrongAnswers >= _quizSettings.NumberOfQuestions)
            throw new Exception("NumberOfQuestions cannot be smaller than MaxNumberOfWrongAnswers!");

        var conn = _configuration.GetConnectionString("QuizDatabase");
        if (string.IsNullOrWhiteSpace(conn))
            throw new Exception("There is no connection string for QuizDatabase!");

        return Task.FromResult(HealthCheckResult.Healthy("Config OK"));
    }
}