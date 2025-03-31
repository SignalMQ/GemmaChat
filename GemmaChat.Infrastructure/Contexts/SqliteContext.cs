using GemmaChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GemmaChat.Infrastructure.Contexts
{
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                Database.Migrate();
            }
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Converstations { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<User>()
                .HasIndex(e => e.ChatId);

            base.OnModelCreating(modelBuilder);
        }
    }
}