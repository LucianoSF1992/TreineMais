using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreineMais.Models
{
    public class Treino
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty; // Ex: Treino A

        [Required]
        public string DiaSemana { get; set; } = string.Empty;

        // Relacionamento com aluno
        [Required]
        public string AlunoId { get; set; } = string.Empty;

        [ForeignKey("AlunoId")]
        public ApplicationUser? Aluno { get; set; }

        // Relacionamento com instrutor
        [Required]
        public string InstrutorId { get; set; } = string.Empty;

        public ApplicationUser? Instrutor { get; set; }

        public bool Concluido { get; set; } = false;

        // Lista de exercícios da ficha
        public ICollection<TreinoExercicio>? TreinosExercicios { get; set; }
    }
}