using Microsoft.EntityFrameworkCore;
using MyMvcBackend.Models;

namespace MyMvcBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<ListeningTest> ListeningTests { get; set; }
        public DbSet<ListeningRecording> ListeningRecordings { get; set; }
        public DbSet<ListeningQuestion> ListeningQuestions { get; set; }
        public DbSet<ListeningPart> ListeningParts { get; set; }
        public DbSet<ListeningAnswer> ListeningAnswers { get; set; }
        public DbSet<UserTestResults> UserTestResults { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ListeningTest -> ListeningRecording (1-n)
            modelBuilder.Entity<ListeningRecording>()
                .HasOne(r => r.ListeningTest)
                .WithMany(t => t.ListeningRecordings)
                .HasForeignKey(r => r.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            // ListeningRecording -> ListeningPart (1-n)
            modelBuilder.Entity<ListeningPart>()
                .HasOne(p => p.ListeningRecording)
                .WithMany(r => r.ListeningParts)
                .HasForeignKey(p => p.RecordingId)
                .OnDelete(DeleteBehavior.Cascade);

            // ListeningPart -> ListeningQuestion (1-n)
            modelBuilder.Entity<ListeningQuestion>()
                .HasOne(q => q.ListeningPart)
                .WithMany(p => p.ListeningQuestions)
                .HasForeignKey(q => q.PartId)
                .OnDelete(DeleteBehavior.Cascade);

            // ListeningQuestion -> ListeningAnswer (1-n)
            modelBuilder.Entity<ListeningAnswer>()
                .HasOne(a => a.ListeningQuestion)
                .WithMany(q => q.ListeningAnswers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserTestResults -> ListeningTest (n-1)
            modelBuilder.Entity<UserTestResults>()
                .HasOne(r => r.ListeningTest)
                .WithMany(t => t.UserTestResults)
                .HasForeignKey(r => r.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserTestResults -> User (n-1)
            modelBuilder.Entity<UserTestResults>()
                .HasOne(r => r.User)
                .WithMany(u => u.UserTestResults)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserAnswer -> UserTestResults (n-1)
            modelBuilder.Entity<UserAnswer>()
                .HasOne(a => a.UserTestResult)
                .WithMany(r => r.UserAnswers)
                .HasForeignKey(a => a.TestResultId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
