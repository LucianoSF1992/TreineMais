using TreineMais.Models;

namespace TreineMais.ViewModels.Dashboard
{
    public class AlunoDashboardViewModel
    {
        public int TotalTreinos { get; set; }
        public List<Treino> MeusTreinos { get; set; } = new();
    }
}