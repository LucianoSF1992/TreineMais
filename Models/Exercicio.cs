namespace TreineMais.Models
{
    public class Exercicio
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? GrupoMuscular { get; set; }

        public string? Descricao { get; set; }

        public ICollection<TreinoExercicio>? TreinosExercicios { get; set; }
    }
}