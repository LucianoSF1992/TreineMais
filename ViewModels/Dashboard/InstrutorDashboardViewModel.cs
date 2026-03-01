using TreineMais.Models;

namespace TreineMais.ViewModels.Dashboard
{
    public class InstrutorDashboardViewModel
    {
        public int TotalAlunos { get; set; }
        public int TotalTreinos { get; set; }

        public List<ApplicationUser> Alunos { get; set; } = new();
        public List<Treino> MeusTreinos { get; set; } = new();
    }
}