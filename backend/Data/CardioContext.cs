using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class CardioContext : DbContext
    {
        public CardioContext(DbContextOptions<CardioContext> options) : base(options) { }
        public DbSet<User> Users { set; get; }
        public DbSet<Folder> Folders { set; get; }
        public DbSet<Word> Words { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Folders)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Words)
                .WithOne(w => w.Folder)
                .HasForeignKey(w => w.FolderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}