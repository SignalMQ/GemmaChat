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
        public DbSet<Content> Contents { get; set; }
        public DbSet<ImageUrl> ImageUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<Conversation>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<Message>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<Content>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<ImageUrl>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<User>()
                .HasIndex(e => e.ChatId);

            base.OnModelCreating(modelBuilder);
        }
    }
}