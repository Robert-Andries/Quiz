using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuestionService;

public class RemoveQuestionCorrectAnswers
{
    [Fact]
    public async Task IntendedUseCase()
    {
        // Arrange
        var question = new Question
        {
            Id = 1,
            CorectOption = new List<int> { 1, 2, 3 }
        };
        var questionFromRepo = new Question
        {
            Id = 1
        };
        var moqQuestionRepository = new Mock<IQuestionRepository>();
        moqQuestionRepository
            .Setup(x => x.GetQuestionById(question.Id))
            .ReturnsAsync(questionFromRepo);

        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(moqQuestionRepository.Object);

        // Act
        await questionService.RemoveQuestionCorrectAnswers(question);

        // Assert
        question.CorectOption.Should().BeEmpty();
    }
}