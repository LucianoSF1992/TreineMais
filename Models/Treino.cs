using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreineMais.Models
{
    public class Treino
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string DiaSemana { get; set; } = string.Empty;

        // Relacionamento com aluno
        [Required]
        public string AlunoId { get; set; } = string.Empty;

        [ForeignKey(nameof(AlunoId))]
        public ApplicationUser? Aluno { get; set; }

        // Relacionamento com instrutor
        [Required]
        public string InstrutorId { get; set; } = string.Empty;

        [ForeignKey(nameof(InstrutorId))]
        public ApplicationUser? Instrutor { get; set; }

        public bool Concluido { get; set; } = false;

        // Evita null
        public ICollection<TreinoExercicio> TreinosExercicios { get; set; } = new List<TreinoExercicio>();
    }
}