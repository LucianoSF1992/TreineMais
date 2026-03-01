namespace TreineMais.ViewModels.Dashboard
{
    public class InstrutorDashboardViewModel
    {
        public int TotalTreinos { get; set; }
        public int TotalAlunos { get; set; }

        public List<AlunoResumoViewModel> MeusAlunos { get; set; } = new();
        public List<TreinoResumoViewModel> UltimosTreinos { get; set; } = new();
    }

    public class AlunoResumoViewModel
    {
        public string AlunoId { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TreinosDoAluno { get; set; }
        public int Pendentes { get; set; }
        public int Concluidos { get; set; }
    }

    public class TreinoResumoViewModel
    {
        public int TreinoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string DiaSemana { get; set; } = string.Empty;
        public string AlunoNome { get; set; } = string.Empty;
        public bool Concluido { get; set; }
    }
}