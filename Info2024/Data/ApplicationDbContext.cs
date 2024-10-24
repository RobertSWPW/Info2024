using Info2024.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Info2024.Data
{
  public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

		public DbSet<Category> Categories { get; set; }
		public DbSet<Text> Texts { get; set; }
		public DbSet<Opinion> Opinions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Alternatywna konfiguracja relacji przez Fluent API
			modelBuilder.Entity<Text>()
					.HasOne(t => t.Category)
					.WithMany(c => c.Texts)
					.HasForeignKey(t => t.CategoryId)
					.OnDelete(DeleteBehavior.Restrict); 
					//uniemożliwia usunięcie kategorii, gdy są do niej przypisane teksty

			modelBuilder.Entity<Text>()
					.HasOne(t => t.User)
					.WithMany(u => u.Texts)
					.HasForeignKey(t => t.Id)
					.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Opinion>()
					.HasOne(o => o.Text)
					.WithMany(t => t.Opinions)
					.HasForeignKey(o => o.TextId)
					.OnDelete(DeleteBehavior.Cascade);
					//usunięcie tekstu usuwa wszystki opinie do niego przypisane

			modelBuilder.Entity<Opinion>()
					.HasOne(o => o.User)
					.WithMany(u => u.Opinions)
					.HasForeignKey(o => o.Id)
					.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
