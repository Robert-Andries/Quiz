using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Quiz.PersistenceLayer.DbContexts;

namespace Quiz.Tests.Api;

public class CustomWebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<QuestionsDbContext>(options =>
            {
                //options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}