using TreineMais.Models;

namespace TreineMais.ViewModels
{
    public class CriarTreinoViewModel
    {
        public int AlunoId { get; set; }
        public string NomeTreino { get; set; } = string.Empty;

        public List<ApplicationUser> Alunos { get; set; } = new();
    }
}