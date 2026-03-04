using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private IActionResult RedirectBackOr(string action, string controller, object? routeValues = null)
        {
            var referer = Request.Headers.Referer.ToString();
            if (!string.IsNullOrWhiteSpace(referer))
                return Redirect(referer);

            return RedirectToAction(action, controller, routeValues);
        }

        // ===================== ALUNO =====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Concluir(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == id);
            if (treino == null) return NotFound();

            if (treino.AlunoId != user.Id)
                return Forbid();

            treino.Concluido = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Aluno", "Dashboard");
        }

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

        // ===================== INSTRUTOR =====================

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> DoAluno(string alunoId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (string.IsNullOrWhiteSpace(alunoId))
                return BadRequest();

            var aluno = await _userManager.FindByIdAsync(alunoId);
            if (aluno == null) return NotFound();

            // ✅ valida por Role, sem TipoUsuario
            var isAluno = await _userManager.IsInRoleAsync(aluno, "Aluno");
            if (!isAluno) return NotFound();

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

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> Detalhe(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.TreinosExercicios)
                    .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null) return NotFound();

            if (treino.InstrutorId != user.Id)
                return Forbid();

            var exerciciosDisponiveis = await _context.Exercicios
                .OrderBy(e => e.Nome)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = string.IsNullOrWhiteSpace(e.GrupoMuscular) ? e.Nome : $"{e.Nome} ({e.GrupoMuscular})"
                })
                .ToListAsync();

            var vm = new TreinoDetalheViewModel
            {
                TreinoId = treino.Id,
                NomeTreino = treino.Nome,
                DiaSemana = treino.DiaSemana,
                AlunoNome = treino.Aluno?.NomeCompleto ?? "Aluno",
                AlunoEmail = treino.Aluno?.Email ?? "",
                ExerciciosDisponiveis = exerciciosDisponiveis,
                ExerciciosDoTreino = treino.TreinosExercicios
                    .OrderBy(x => x.Ordem)
                    .ThenBy(x => x.Id)
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> AdicionarExercicio(TreinoDetalheViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == model.TreinoId);
            if (treino == null) return NotFound();

            if (treino.InstrutorId != user.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Detalhe), new { id = model.TreinoId });

            var exercicioExiste = await _context.Exercicios.AnyAsync(e => e.Id == model.ExercicioId);
            if (!exercicioExiste)
                return RedirectToAction(nameof(Detalhe), new { id = model.TreinoId });

            var te = new TreinoExercicio
            {
                TreinoId = model.TreinoId,
                ExercicioId = model.ExercicioId,
                Series = model.Series,
                Repeticoes = model.Repeticoes,
                Descanso = model.Descanso,
                Observacoes = string.IsNullOrWhiteSpace(model.Observacoes) ? null : model.Observacoes.Trim(),
                Ordem = model.Ordem
            };

            _context.TreinoExercicios.Add(te);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["Erro"] = "Já existe um exercício com essa ordem nesse treino. Altere a ordem e tente novamente.";
            }

            return RedirectToAction(nameof(Detalhe), new { id = model.TreinoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> RemoverExercicio(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var te = await _context.TreinoExercicios.FirstOrDefaultAsync(x => x.Id == id);
            if (te == null) return NotFound();

            var treino = await _context.Treinos.FirstOrDefaultAsync(t => t.Id == te.TreinoId);
            if (treino == null) return NotFound();

            if (treino.InstrutorId != user.Id)
                return Forbid();

            _context.TreinoExercicios.Remove(te);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detalhe), new { id = te.TreinoId });
        }

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

            return RedirectBackOr("Instrutor", "Dashboard");
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

            return RedirectBackOr("Instrutor", "Dashboard");
        }

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

            return RedirectBackOr("Instrutor", "Dashboard");
        }
    }
}