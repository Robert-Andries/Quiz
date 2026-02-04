using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.PersistenceLayer.DbContexts;

namespace Quiz.ApiTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptors = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<QuestionsDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(QuestionsDbContext)).ToList();

            foreach (var d in descriptors)
            {
                services.Remove(d);
            }

            services.AddScoped<DbContextOptions<QuestionsDbContext>>(sp => 
            {
                return new DbContextOptionsBuilder<QuestionsDbContext>()
                    .UseInMemoryDatabase("InMemoryDbForTesting")
                    .Options;
            });

            services.AddScoped<QuestionsDbContext>(sp => 
            {
                var options = sp.GetRequiredService<DbContextOptions<QuestionsDbContext>>();
                return new QuestionsDbContext(options);
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<QuestionsDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

                db.Database.EnsureCreated();

                try
                {
                    var questions = new List<Quiz.DomainLayer.Entities.Question>
                    {
                        new Quiz.DomainLayer.Entities.Question(0, "Q1?", null, new List<string> { "A", "B" }, new List<int> { 0, 1 }, Quiz.DomainLayer.Entities.QuestionType.Choice),
                        new Quiz.DomainLayer.Entities.Question(0, "Q2?", null, new List<string> { "C", "D" }, new List<int> { 0, 1 }, Quiz.DomainLayer.Entities.QuestionType.Choice),
                        new Quiz.DomainLayer.Entities.Question(0, "Q3?", null, new List<string> { "E", "F" }, new List<int> { 0, 1 }, Quiz.DomainLayer.Entities.QuestionType.Choice)
                    };
                    db.Questions.AddRange(questions);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }
}
