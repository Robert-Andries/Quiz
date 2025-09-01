using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quiz.PersistenceLayer.DbContexts;
using Quiz.WebApi.StartupConfig;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<QuestionsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:QuizDatabase"]);
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQuizVersioning();
builder.Services.AddResponseCaching();
builder.AddQuizRateLimiting();
builder.AddDependencyInjection();
builder.AddHealthChecks();


builder.Services.AddSerilog(opts => { opts.WriteTo.Console(); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.UseIpRateLimiting();

app.Run();

public partial class Program
{
}