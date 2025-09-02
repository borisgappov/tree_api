using Microsoft.EntityFrameworkCore;
using TreeApi.Data.Entities;

namespace TreeApi.Data
{
    public class TreeApiDbContext : DbContext
    {
        public TreeApiDbContext(DbContextOptions<TreeApiDbContext> options) : base(options)
        {
        }
        
        public DbSet<Tree> Trees { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<ExceptionJournal> ExceptionJournals { get; set; }
        public DbSet<Partner> Partners { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Tree>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Node>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.Tree)
                    .WithMany(e => e.Nodes)
                    .HasForeignKey(e => e.TreeId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Parent)
                    .WithMany(e => e.Children)
                    .HasForeignKey(e => e.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<ExceptionJournal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.EventId).IsRequired();
                entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.StackTrace).IsRequired();
            });
            
            modelBuilder.Entity<Partner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Code).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.Code).IsUnique();
            });
        }
    }
}
