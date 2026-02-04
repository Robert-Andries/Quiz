using Microsoft.EntityFrameworkCore;
using Quiz.PersistenceLayer.DbContexts;

namespace Quiz.WebApi.StartupConfig;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var questionsDbContext = scope.ServiceProvider.GetRequiredService<QuestionsDbContext>();
        questionsDbContext.Database.Migrate();
    }
}