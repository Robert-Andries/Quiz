using Microsoft.EntityFrameworkCore;
using Quiz.DomainLayer.Interfaces;
using Quiz.PersistenceLayer.DbContexts;

namespace Quiz.PersistenceLayer;

public class QuizRepository : IQuizRepository
{
    private readonly QuizDbContext _context;

    public QuizRepository(QuizDbContext context)
    {
        _context = context;
    }

    public async Task<DomainLayer.Entities.Quiz> GetQuizById(int id)
    {
        var quiz = await _context.Quizzes.FindAsync(id);
        if (quiz == null)
            throw new NullReferenceException("Quiz not found");
        await _context.Entry(quiz).Collection(x => x.Questions).LoadAsync();
        await _context.Entry(quiz).Collection(x => x.Answer).LoadAsync();
        return quiz;
    }

    public async Task SaveQuiz(DomainLayer.Entities.Quiz quiz)
    {
        if (quiz.Id == 0)
        {
            _context.Quizzes.Add(quiz); // children with Id=0 will also be inserted
            await _context.SaveChangesAsync();
            return;
        }

        // Existing -> go to pattern #2
        await UpdateExistingQuiz(quiz);
    }

    public List<DomainLayer.Entities.Quiz> GetQuizzes()
    {
        return _context.Quizzes.ToList();
    }

    private async Task UpdateExistingQuiz(DomainLayer.Entities.Quiz incoming)
    {
        var dbQuiz = await _context.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == incoming.Id);

        if (dbQuiz == null)
        {
            // Treat as new
            _context.Quizzes.Add(incoming);
            await _context.SaveChangesAsync();
            return;
        }

        dbQuiz.IsComplete = incoming.IsComplete;

        foreach (var incomingQ in incoming.Questions.Where(q => q.Id != 0))
        {
            var dbQ = dbQuiz.Questions.FirstOrDefault(x => x.Id == incomingQ.Id);
            if (dbQ != null)
            {
                dbQ.Options = incomingQ.Options;
                dbQ.CorectOption = incomingQ.CorectOption;
                dbQ.Text = incomingQ.Text;
                dbQ.TextSecond = incomingQ.TextSecond;
                dbQ.Type = incomingQ.Type;
            }
        }

        foreach (var newQ in incoming.Questions.Where(q => q.Id == 0)) dbQuiz.Questions.Add(newQ);

        var incomingIds = incoming.Questions.Where(q => q.Id != 0).Select(q => q.Id).ToHashSet();
        var toRemove = dbQuiz.Questions.Where(q => q.Id != 0 && !incomingIds.Contains(q.Id)).ToList();
        foreach (var r in toRemove)
            _context.Remove(r);

        await _context.SaveChangesAsync();
    }
}