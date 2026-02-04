using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuizService;

public class AddQuizCorrectAnswers
{
    [Fact]
    public async Task ShouldAddCorrectAnswersFromQuiz()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);
        var questions = new List<Question>
        {
            new()
            {
                Id = 1,
                CorectOption = [0, 1]
            },
            new()
            {
                Id = 2,
                CorectOption = [0]
            },
            new()
            {
                Id = 3,
                CorectOption = [3]
            }
        };
        var quiz = new DomainLayer.Entities.Quiz([
            new Question
            {
                Id = 1,
                CorectOption = new List<int>()
            },

            new Question
            {
                Id = 2,
                CorectOption = new List<int>()
            },

            new Question
            {
                Id = 3,
                CorectOption = new List<int>()
            }
        ]);
        quiz.NextQuestion();
        quiz.NextQuestion();
        quiz.NextQuestion();

        mockRepository.Setup(repo => repo.GetQuestionById(It.IsAny<int>()))
            .ReturnsAsync((int id) => questions.FirstOrDefault(q => q.Id == id));

        // Act
        await quizService.AddQuizCorrectAnswers(quiz);

        // Assert
        foreach (var question in quiz.Questions)
            question.CorectOption.Count.Should().BeGreaterThan(0);
    }
}