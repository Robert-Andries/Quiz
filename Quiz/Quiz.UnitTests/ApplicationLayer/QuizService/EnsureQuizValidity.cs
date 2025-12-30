using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuizService;

public class EnsureQuizValidity
{
    [Fact]
    public async Task ShouldReturnArgumentNullException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => quizService.EnsureQuizValidity(null!));
    }

    [Fact]
    public async Task ShouldReturnArgumentException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 1);
        var quiz = new DomainLayer.Entities.Quiz(new List<Question>
        {
            new Question { Id = 1, Text = "Question 1" },
            new Question { Id = 2, Text = "Question 2" }
        });
        quiz.Questions.RemoveAll(q => true); //Simulating an empty quiz

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => quizService.EnsureQuizValidity(quiz));
    }

    [Fact]
    public async Task ShouldReturnInvalidOperationException()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 1);
        var quiz = new DomainLayer.Entities.Quiz(new List<Question>
        {
            new Question { Id = 1, Text = "Question 1" },
            new Question { Id = 2, Text = "Question 2" }
        });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => quizService.EnsureQuizValidity(quiz));
    }

    [Fact]
    public async Task ShouldNotPassQuestionsCheck()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 1);
        var quiz = new DomainLayer.Entities.Quiz(new List<Question>
        {
            new Question { Id = 1, Text = "Question 1" },
            new Question { Id = 2, Text = "Question 2" }
        });

        mockRepository.Setup(repo => repo.GetQuestionById(1)).ReturnsAsync(quiz.Questions.ElementAt(0));
        mockRepository.Setup(repo => repo.GetQuestionById(2)).ReturnsAsync((Question?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => quizService.EnsureQuizValidity(quiz, false));
    }

    [Fact]
    public async Task ShouldNotThrow()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 1);
        var quiz = new DomainLayer.Entities.Quiz(new List<Question>
        {
            new Question { Id = 1, Text = "Question 1" },
            new Question { Id = 2, Text = "Question 2" }
        });

        mockRepository.Setup(repo => repo.GetQuestionById(1)).ReturnsAsync(quiz.Questions.ElementAt(0));
        mockRepository.Setup(repo => repo.GetQuestionById(2)).ReturnsAsync(quiz.Questions.ElementAt(1));

        quiz.NextQuestion();
        quiz.NextQuestion();

        // Act
        var exception = await Record.ExceptionAsync(() => quizService.EnsureQuizValidity(quiz));

        // Assert
        Assert.Null(exception);
    }
}