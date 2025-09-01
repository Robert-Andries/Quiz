using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Quiz.DomainLayer.Value_Objects;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuizService;

public class CalculateStatistics
{
    [Fact]
    public async Task ShouldCalculateCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<IQuestionRepository>();
        var quizService = new Quiz.ApplicationLayer.Services.QuizService(mockRepository.Object, 3);
        var questions = new List<Question>
        {
            new() { Id = 1, CorectOption = new List<int> { 1 } },
            new() { Id = 2, CorectOption = new List<int> { 2 } },
            new() { Id = 3, CorectOption = new List<int> { 3 } },
            new() { Id = 4, CorectOption = new List<int> { 1 } },
            new() { Id = 5, CorectOption = new List<int> { 1 } }
        };
        var quiz = new DomainLayer.Entities.Quiz(questions);
        quiz.Answer.Add(new SelectedAnswer(1, new List<int> { 1 }));
        quiz.Answer.Add(new SelectedAnswer(2, new List<int> { 99 }));
        quiz.Answer.Add(new SelectedAnswer(3, new List<int> { 99 }));
        quiz.Answer.Add(new SelectedAnswer(4, new List<int> { 1 }));
        quiz.Answer.Add(new SelectedAnswer(5, new List<int> { 1 }));

        quiz.NextQuestion();
        quiz.NextQuestion();
        quiz.NextQuestion();
        quiz.NextQuestion();
        quiz.NextQuestion();

        mockRepository.Setup(repo => repo.GetQuestionById(It.IsAny<int>()))
            .ReturnsAsync((int id) => questions.FirstOrDefault(q => q.Id == id));

        // Act
        var statistics = await quizService.CalculateStatistics(quiz);

        // Assert
        statistics.NumberOfQuestions.Should().Be(quiz.Questions.Count);
        statistics.CorrectAnswers.Should().Be(3);
        statistics.WrongAnswers.Should().Be(2);
        statistics.IsPassed.Should().BeTrue();
    }
}