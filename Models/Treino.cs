using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreineMais.Models
{
    public class Treino
    {
        public int Id { get; set; }

        [Required]
        public string? DiaSemana { get; set; }

        [Required]
        public string? NomeExercicio { get; set; }

        public string? GrupoMuscular { get; set; }

        public int Series { get; set; }

        public int Repeticoes { get; set; }

        public int Descanso { get; set; }

        public string? Observacoes { get; set; }

        // Relacionamento com aluno
        [Required]
        public string? AlunoId { get; set; }

        [ForeignKey("AlunoId")]
        public ApplicationUser? Aluno { get; set; }

        public bool Concluido { get; set; } = false;
    }
}