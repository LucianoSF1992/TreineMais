using System.ComponentModel.DataAnnotations;

namespace TreineMais.Models
{
    public class Exercicio
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(80)]
        public string? GrupoMuscular { get; set; }

        [MaxLength(400)]
        public string? Descricao { get; set; }

        // Evita null
        public ICollection<TreinoExercicio> TreinosExercicios { get; set; } = new List<TreinoExercicio>();
    }
}