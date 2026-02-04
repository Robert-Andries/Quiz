using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuestionService;

public class AddQuestionCorrectAnswers
{
    [Fact]
    public async Task IntendedUseCase()
    {
        // Arrange
        var question = new Question
        {
            Id = 1,
            CorectOption = new List<int>()
        };
        var questionFinal = new Question
        {
            Id = 1,
            CorectOption = new List<int> { 1, 2, 3 }
        };
        var moqQuestionRepository = new Mock<IQuestionRepository>();
        moqQuestionRepository
            .Setup(x => x.GetQuestionById(question.Id))
            .ReturnsAsync(questionFinal);

        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(moqQuestionRepository.Object);

        // Act
        await questionService.AddQuestionCorrectAnswers(question);

        // Assert
        question.CorectOption.Should().NotBeNullOrEmpty();
        question.CorectOption.Should().BeEquivalentTo(questionFinal.CorectOption);
    }
}