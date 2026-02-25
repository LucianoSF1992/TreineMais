namespace TreineMais.Models
{
    public class TreinoExercicio
    {
        public int Id { get; set; }

        public int TreinoId { get; set; }
        public Treino? Treino { get; set; }

        public int ExercicioId { get; set; }
        public Exercicio? Exercicio { get; set; }

        public int Series { get; set; }

        public int Repeticoes { get; set; }

        public int Descanso { get; set; } // em segundos

        public string? Observacoes { get; set; }

        public int Ordem { get; set; }
    }
}