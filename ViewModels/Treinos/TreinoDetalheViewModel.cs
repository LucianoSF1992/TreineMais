using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TreineMais.Models;

namespace TreineMais.ViewModels.Treinos
{
    public class TreinoDetalheViewModel
    {
        public int TreinoId { get; set; }
        public string NomeTreino { get; set; } = string.Empty;
        public string DiaSemana { get; set; } = string.Empty;

        public string AlunoNome { get; set; } = string.Empty;
        public string AlunoEmail { get; set; } = string.Empty;

        // Lista já vinculada (com Exercicio incluído)
        public List<TreinoExercicio> ExerciciosDoTreino { get; set; } = new();

        // Dropdown
        public List<SelectListItem> ExerciciosDisponiveis { get; set; } = new();

        // Form para adicionar
        [Required(ErrorMessage = "Selecione um exercício.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um exercício.")]
        public int ExercicioId { get; set; }

        [Range(0, 50)]
        public int Series { get; set; }

        [Range(0, 200)]
        public int Repeticoes { get; set; }

        [Range(0, 3600)]
        public int Descanso { get; set; }

        [MaxLength(250)]
        public string? Observacoes { get; set; }

        [Range(0, 999)]
        public int Ordem { get; set; }
    }
}