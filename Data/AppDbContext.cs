using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TreineMais.Models;

namespace TreineMais.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Treino> Treinos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Treino>()
                .HasOne(t => t.Instrutor)
                .WithMany()
                .HasForeignKey(t => t.InstrutorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}