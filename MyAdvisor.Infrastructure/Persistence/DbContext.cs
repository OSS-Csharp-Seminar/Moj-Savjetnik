using Microsoft.EntityFrameworkCore;
using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<FinancialDiary> FinancialDiaries => Set<FinancialDiary>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionAiLog> TransactionAiLogs => Set<TransactionAiLog>();
        public DbSet<RecurringTransaction> RecurringTransactions => Set<RecurringTransaction>();
        public DbSet<SpendingStatistic> SpendingStatistics => Set<SpendingStatistic>();
        public DbSet<CategoryStatistic> CategoryStatistics => Set<CategoryStatistic>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId);

            modelBuilder.Entity<FinancialDiary>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Diary)
                .WithMany(d => d.Transactions)
                .HasForeignKey(t => t.DiaryId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<TransactionAiLog>()
                .HasOne(l => l.Transaction)
                .WithMany()
                .HasForeignKey(l => l.TransactionId);

            modelBuilder.Entity<TransactionAiLog>()
                .HasOne(l => l.AiCategory)
                .WithMany()
                .HasForeignKey(l => l.AiCategoryId);

            modelBuilder.Entity<RecurringTransaction>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<RecurringTransaction>()
                .HasOne(r => r.Category)
                .WithMany()
                .HasForeignKey(r => r.CategoryId);

            modelBuilder.Entity<SpendingStatistic>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<CategoryStatistic>()
                .HasOne(cs => cs.Statistics)
                .WithMany(s => s.CategoryBreakdown)
                .HasForeignKey(cs => cs.StatisticsId);

            modelBuilder.Entity<CategoryStatistic>()
                .HasOne(cs => cs.Category)
                .WithMany()
                .HasForeignKey(cs => cs.CategoryId);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Token)
                    .IsRequired();

                entity.HasIndex(x => x.Token)
                    .IsUnique();

                entity.HasOne<User>() 
                    .WithMany()
                    .HasForeignKey(x => x.UserId);
            });
        }
    }
}
