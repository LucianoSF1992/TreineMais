using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;

namespace TreineMais.Controllers
{
    [Authorize]
    public class TreinosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public TreinosController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ✅ Aluno pode marcar como concluído SOMENTE o treino dele
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Concluir(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == id);
            if (treino == null) return NotFound();

            // segurança: só o dono do treino
            if (treino.AlunoId != user.Id)
                return Forbid();

            treino.Concluido = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Aluno", "Dashboard");
        }

        // ✅ opcional: permitir "desmarcar" (se quiser)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Reabrir(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == id);
            if (treino == null) return NotFound();

            if (treino.AlunoId != user.Id)
                return Forbid();

            treino.Concluido = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Aluno", "Dashboard");
        }
    }
}