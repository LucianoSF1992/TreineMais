using TreineMais.Models;

namespace TreineMais.ViewModels.Treinos
{
    public class AlunoTreinosViewModel
    {
        public string AlunoId { get; set; } = string.Empty;
        public string AlunoNome { get; set; } = string.Empty;
        public string AlunoEmail { get; set; } = string.Empty;

        public List<Treino> Treinos { get; set; } = new();
    }
}