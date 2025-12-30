using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuizService;

public class RemoveQuizCorrectAnswers
{
    [Fact]
    public async Task ShouldRemoveCorrectAnswersFromQuiz()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);
        var quiz = new DomainLayer.Entities.Quiz(new List<Question>
        {
            new()
            {
                Id = 1,
                CorectOption = new List<int> { 0, 1 }
            },
            new()
            {
                Id = 2,
                CorectOption = new List<int> { 0 }
            },
            new()
            {
                Id = 3,
                CorectOption = new List<int> { 3 }
            }
        });

        mockRepository.Setup(repo => repo.GetQuestionById(It.IsAny<int>()))
            .ReturnsAsync((int id) => quiz.Questions.FirstOrDefault(q => q.Id == id));

        // Act
        await quizService.RemoveQuizCorrectAnswers(quiz);

        // Assert
        foreach (var question in quiz.Questions)
            question.CorectOption.Should().BeEmpty();
        mockRepository.Verify(x => x.GetQuestionById(It.IsAny<int>()), Times.Exactly(quiz.Questions.Count * 2));
    }
}