using Microsoft.EntityFrameworkCore;
using Quiz.DomainLayer.Entities;

namespace Quiz.PersistenceLayer.DbContexts;

public class QuestionsDbContext : DbContext
{
    public QuestionsDbContext(DbContextOptions<QuestionsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; }
}