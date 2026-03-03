using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreineMais.Models
{
    public class TreinoExercicio
    {
        public int Id { get; set; }

        [Required]
        public int TreinoId { get; set; }

        [ForeignKey(nameof(TreinoId))]
        public Treino? Treino { get; set; }

        [Required]
        public int ExercicioId { get; set; }

        [ForeignKey(nameof(ExercicioId))]
        public Exercicio? Exercicio { get; set; }

        [Range(0, 50)]
        public int Series { get; set; }

        [Range(0, 200)]
        public int Repeticoes { get; set; }

        [Range(0, 3600)]
        public int Descanso { get; set; } // segundos

        [MaxLength(250)]
        public string? Observacoes { get; set; }

        [Range(0, 999)]
        public int Ordem { get; set; }
    }
}