using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TreineMais.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(120)]
        public string? NomeCompleto { get; set; }

        public int? Idade { get; set; }

        [StringLength(200)]
        public string? Objetivo { get; set; }

        [StringLength(20)]
        public string? TipoUsuario { get; set; } // Admin | Instrutor | Aluno

        // ✅ RELAÇÃO COM INSTRUTOR
        public string? InstrutorId { get; set; }

        public ApplicationUser? Instrutor { get; set; }
    }
}