using TreineMais.Models;

namespace TreineMais.ViewModels
{
    public class CriarTreinoViewModel
    {
        public string? AlunoId { get; set; } = string.Empty;

        public string NomeTreino { get; set; } = string.Empty;

        public string DiaSemana { get; set; } = string.Empty;

        public string NomeExercicio { get; set; } = string.Empty;

        public string GrupoMuscular { get; set; } = string.Empty;

        public int Series { get; set; }

        public int Repeticoes { get; set; }

        public string Descanso { get; set; } = string.Empty;

        public string Observacoes { get; set; } = string.Empty;

        public List<ApplicationUser> Alunos { get; set; } = new();
    }
}