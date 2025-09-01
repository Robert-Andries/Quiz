using FluentAssertions;
using Xunit;

namespace Quiz.Tests.Api.Controller;

public class QuestionControllerTest : IClassFixture<CustomWebApiFactory>
{
    private readonly CustomWebApiFactory _factory;

    public QuestionControllerTest(CustomWebApiFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GetQuestions_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/Questions");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Should().NotBeNull();
    }
}