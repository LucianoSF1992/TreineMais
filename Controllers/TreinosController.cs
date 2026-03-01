using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;
using TreineMais.ViewModels.Treinos;

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

        // ✅ Instrutor vê treinos de um aluno (apenas os treinos dele)
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> DoAluno(string alunoId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (string.IsNullOrWhiteSpace(alunoId))
                return BadRequest();

            var aluno = await _context.Users.FirstOrDefaultAsync(u => u.Id == alunoId && u.TipoUsuario == "Aluno");
            if (aluno == null) return NotFound();

            var treinos = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id && t.AlunoId == alunoId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();

            var vm = new AlunoTreinosViewModel
            {
                AlunoId = aluno.Id,
                AlunoNome = aluno.NomeCompleto ?? "Aluno",
                AlunoEmail = aluno.Email ?? "",
                Treinos = treinos
            };

            return View(vm);
        }

        // ✅ Instrutor pode concluir/reabrir treinos que ele criou
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> ConcluirInstrutor(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == id);
            if (treino == null) return NotFound();

            if (treino.InstrutorId != user.Id)
                return Forbid();

            treino.Concluido = true;
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> ReabrirInstrutor(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == id);
            if (treino == null) return NotFound();

            if (treino.InstrutorId != user.Id)
                return Forbid();

            treino.Concluido = false;
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers.Referer.ToString());
        }

        // ✅ Instrutor pode excluir treino que ele criou
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> ExcluirInstrutor(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == id);
            if (treino == null) return NotFound();

            if (treino.InstrutorId != user.Id)
                return Forbid();

            _context.Treinos.Remove(treino);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers.Referer.ToString());
        }
    }
}