using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Quiz.DomainLayer.Value_Objects;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuestionService;

public class CheckQuestionResponse
{
    [Fact]
    public async Task ShouldReturnNullException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => questionService.CheckQuestionResponse(null!));
    }

    [Fact]
    public async Task ShouldReturnArgumentException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(mockRepository.Object);
        var answer = new SelectedAnswer { QuestionId = 1, SelectedAnswers = new List<int> { 1 } };

        mockRepository.Setup(repo => repo.GetQuestionById(It.IsAny<int>())).ReturnsAsync((Question)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => questionService.CheckQuestionResponse(answer));
    }

    [Fact]
    public async Task ShouldReturnTrue()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(mockRepository.Object);
        var answer = new SelectedAnswer { QuestionId = 1, SelectedAnswers = new List<int> { 1 } };
        var questionFromRepo = new Question
        {
            Id = 1,
            CorectOption = new List<int> { 1 }
        };

        mockRepository.Setup(repo => repo.GetQuestionById(It.IsAny<int>())).ReturnsAsync(questionFromRepo);

        // Act
        var result = await questionService.CheckQuestionResponse(answer);

        // Assert
        result.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetQuestions()
    {
        yield return new object[] { new Question { Id = 1, CorectOption = new List<int> { 1 } } };
        yield return new object[] { new Question { Id = 1, CorectOption = new List<int> { 1, 2, 3 } } };
    }

    [Theory]
    [MemberData(nameof(GetQuestions))]
    public async Task ShouldReturnFalse(Question questionFromRepo)
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(mockRepository.Object);
        var answer = new SelectedAnswer { QuestionId = 1, SelectedAnswers = new List<int> { 3 } };

        mockRepository.Setup(repo => repo.GetQuestionById(It.IsAny<int>())).ReturnsAsync(questionFromRepo);

        // Act
        var result = await questionService.CheckQuestionResponse(answer);

        // Assert
        result.Should().BeFalse();
    }
}