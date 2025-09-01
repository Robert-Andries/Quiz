using Quiz.DomainLayer.Entities;

namespace Quiz.DomainLayer.Interfaces;

public interface IQuestionRepository
{
    Task<Question?> GetQuestionById(int id);
    Task<List<Question>> GetAllQuestions();
}