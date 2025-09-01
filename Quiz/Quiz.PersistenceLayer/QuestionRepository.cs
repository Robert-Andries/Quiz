using Microsoft.EntityFrameworkCore;
using Quiz.DomainLayer.Entities;
using Quiz.DomainLayer.Interfaces;
using Quiz.PersistenceLayer.DbContexts;

namespace Quiz.PersistenceLayer;

public class QuestionRepository : IQuestionRepository
{
    private readonly QuestionsDbContext _context;

    public QuestionRepository(QuestionsDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetQuestionById(int id)
    {
        return await _context.Questions.FindAsync(id);
    }

    public async Task<List<Question>> GetAllQuestions()
    {
        return await _context.Questions.ToListAsync();
    }
}