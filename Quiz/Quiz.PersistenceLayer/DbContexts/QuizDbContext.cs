using Microsoft.EntityFrameworkCore;

namespace Quiz.PersistenceLayer.DbContexts;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options)
        : base(options)
    {
    }

    public DbSet<DomainLayer.Entities.Quiz> Quizzes { get; set; }
}