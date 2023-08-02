using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCCore.Models;

namespace MVCCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AlbumModel> Albums { get; set; }
        public DbSet<PhotoModel> Photos { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<StatsModel> Stats { get; set; }
        public DbSet<CoverModel> Covers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CoverModel>()
                .HasKey(c => c.ID);

            builder.Entity<AlbumModel>()
                .HasMany(a => a.DbPhotos)
                .WithOne(p => p.Album)
                .HasForeignKey(p => p.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AlbumModel>()
                .Property(a => a.Category)
                .HasConversion<string>();

            builder.Entity<StatsModel>()
                .HasKey(s => s.Id);
                
        }
    }
}