using FilmsCatalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilmsCatalog.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Film> Films { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Film>()
                .Property(f => f.Name)
                .IsRequired();

            modelBuilder.Entity<Film>()
                .Property(f => f.Producer)
                .IsRequired();

            modelBuilder.Entity<Film>()
                .Property(f => f.Poster)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
