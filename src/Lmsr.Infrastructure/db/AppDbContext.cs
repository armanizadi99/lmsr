using Lmsr.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
namespace Lmsr.Infrastructure.Db;

public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

public DbSet<Course> Courses {get; set; }
public DbSet<Word> Words {get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
modelBuilder.Entity<Word>()
.HasMany(w => w.Definitions)
.WithOne()
.HasForeignKey(wd => wd.WordId);
modelBuilder.Entity<Course>()
.HasIndex(c => c.Title)
.IsUnique();

modelBuilder.Entity<Course>()
.ToTable(t => {
t.HasCheckConstraint("CK_Course_Title_NotEmpty", "Title <> ''");
t.HasCheckConstraint("CK_Course_UserId_NotEmpty", "UserId <> ''");
});
modelBuilder.Entity<Word>()
.ToTable(t => {
t.HasCheckConstraint("CK_Word_Term_NotEmpty", "Term <> ''");
});
modelBuilder.Entity<WordDefinition>()
.ToTable(t => {
t.HasCheckConstraint("CK_WordDefinition_Text_NotEmpty", "Text <> ''");
});

modelBuilder.Entity<Course>()
.Property(c => c.Title)
.IsRequired();
modelBuilder.Entity<Course>()
.Property(c => c.UserId)
.IsRequired();

modelBuilder.Entity<Word>()
.Property(c => c.Term)
.IsRequired();

modelBuilder.Entity<WordDefinition>()
.Property(c => c.Text)
.IsRequired();
}
}