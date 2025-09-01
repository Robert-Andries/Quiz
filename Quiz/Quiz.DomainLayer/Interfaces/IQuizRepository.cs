namespace Quiz.DomainLayer.Interfaces;

public interface IQuizRepository
{
    public Task<Entities.Quiz> GetQuizById(int id);
    public Task SaveQuiz(Entities.Quiz quiz);
    public List<Entities.Quiz> GetQuizzes();
}