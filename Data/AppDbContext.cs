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
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<TreinoExercicio> TreinosExercicios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Treino>()
                .HasOne(t => t.Instrutor)
                .WithMany()
                .HasForeignKey(t => t.InstrutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TreinoExercicio>()
                .HasOne(te => te.Treino)
                .WithMany(t => t.TreinosExercicios)
                .HasForeignKey(te => te.TreinoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TreinoExercicio>()
                .HasOne(te => te.Exercicio)
                .WithMany(e => e.TreinosExercicios)
                .HasForeignKey(te => te.ExercicioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TreinoExercicio>()
                .HasIndex(te => new { te.TreinoId, te.Ordem })
                .IsUnique();
        }
    }
}