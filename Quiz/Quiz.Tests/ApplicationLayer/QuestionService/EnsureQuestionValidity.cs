using FluentAssertions;
using Moq;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Xunit;

namespace Quiz.Tests.ApplicationLayer.QuestionService;

public class EnsureQuestionValidity
{
    [Fact]
    public async Task ShouldReturnArgumentNullException()
    {
        // Arrange
        var mockQuestionsRepository = new Mock<IQuestionRepository>();
        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(mockQuestionsRepository.Object);

        // Act & Assert
        Func<Task> act = () => questionService.EnsureQuestionValidity(null!);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ShouldReturnArgumentException()
    {
        // Arrange
        var mockQuestionsRepository = new Mock<IQuestionRepository>();
        var questionService = new Quiz.ApplicationLayer.Services.QuestionService(mockQuestionsRepository.Object);
        var question = new Question
        {
            Id = 1
        };
        mockQuestionsRepository
            .Setup(repo => repo.GetQuestionById(question.Id))
            .ReturnsAsync((Question)null!);
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => questionService.EnsureQuestionValidity(question));
    }
}