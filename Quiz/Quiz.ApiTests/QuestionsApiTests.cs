using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Quiz.DomainLayer.Entities;
using Xunit;

namespace Quiz.ApiTests;

public class QuestionsApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public QuestionsApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetQuestionsSet_ReturnsOk_WhenQuestionsExist()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Questions");

        response.EnsureSuccessStatusCode(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostQuiz_ReturnsOk_WhenQuizIsValid()
    {
        // 1. Get a valid quiz first
        var getResponse = await _client.GetAsync("/api/v1/Questions");
        getResponse.EnsureSuccessStatusCode();
        var quiz = await getResponse.Content.ReadFromJsonAsync<DomainLayer.Entities.Quiz>();
        
        // 2. Simulate completing the quiz
        // If we have questions, we need to ensure RemainingQuestions == 0
        // RemainingQuestions = Count - Index - 1
        // So Index should be Count - 1.
        if (quiz != null && quiz.Questions != null && quiz.Questions.Count > 0)
        {
            quiz.CurentQuestionIndex = quiz.Questions.Count - 1;
            // Also need to set IsComplete? EnsureQuizValidity checks RemainingQuestions > 0.
            // It doesn't check IsComplete directly in standard validation unless stated.
            // QuizService.EnsureQuizValidity:
            // if (checkRemainingQuestions && quiz.RemainingQuestions > 0) throw...
        }

        // 3. Post back
        var response = await _client.PostAsJsonAsync("/api/v1/Questions", quiz);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task PostQuiz_ReturnsBadRequest_WhenQuizIsNull()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Questions", (DomainLayer.Entities.Quiz)null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
