using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuizService;

public class GetRandomQuestions
{
    [Fact]
    public void ShoultReturnArgumentNullException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => quizService.GetRandomQuestions(null!, 5));
        Assert.Throws<ArgumentNullException>(() => quizService.GetRandomQuestions(new List<Question>(), 5));
    }

    [Fact]
    public void ShouldReturnArgumentOutOfRangeException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);
        var list = new List<Question>
        {
            new(),
            new()
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => quizService.GetRandomQuestions(list, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => quizService.GetRandomQuestions(list, 99999));
    }

    [Fact]
    public void ShouldReturnCorrectAmountOfQuestions()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);
        var list = new List<Question>
        {
            new(),
            new(),
            new(),
            new(),
            new()
        };

        // Act
        var result1 = quizService.GetRandomQuestions(list, 3).ToList();
        var result2 = quizService.GetRandomQuestions(list, 2).ToList();
        var result3 = quizService.GetRandomQuestions(list, 0).ToList();

        // Assert
        result1.Count.Should().Be(3);
        result2.Count.Should().Be(2);
        result3.Count.Should().Be(0);
    }

    [Fact]
    public void ShouldReturnListThatIsDistinct()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);
        var list = new List<Question>();

        for (var i = 1; i <= 200; i++) list.Add(new Question { Id = i });

        // Act
        var result = quizService.GetRandomQuestions(list, 150).ToList();

        // Assert
        result.Count.Should().Be(result.Distinct().Count());
    }
}