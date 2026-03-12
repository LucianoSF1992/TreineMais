using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;
using TreineMais.ViewModels;
using TreineMais.ViewModels.Instrutores;

namespace TreineMais.Controllers
{
    [Authorize] // ✅ controle por action
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ===================== ALUNOS (INSTRUTOR) =====================

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> Alunos()
        {
            // Lista de alunos (por enquanto: todos na role Aluno)
            // Próxima evolução: filtrar por instrutor (alunos do instrutor logado).
            var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
            return View(alunos.OrderBy(a => a.Email).ToList());
        }

        [Authorize(Roles = "Instrutor")]
        public IActionResult CriarAluno()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> CriarAluno(ApplicationUser model, string senha)
        {
            var instrutor = await _userManager.GetUserAsync(User);

            if (instrutor == null)
                return Challenge();

            var email = model.Email?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(nameof(model.Email), "E-mail é obrigatório.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                ModelState.AddModelError(nameof(senha), "Senha é obrigatória.");
                return View(model);
            }

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Já existe um usuário com este e-mail.");
                return View(model);
            }

            var novoAluno = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NomeCompleto = model.NomeCompleto,
                Idade = model.Idade,
                Objetivo = model.Objetivo,
                TipoUsuario = "Aluno",
                InstrutorId = instrutor.Id
            };

            var result = await _userManager.CreateAsync(novoAluno, senha);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(novoAluno, "Aluno");

            return RedirectToAction(nameof(Alunos));
        }

        // ✅ NOVO: Editar aluno (GET)
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> EditarAluno(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(Alunos));

            var aluno = await _userManager.FindByIdAsync(id);
            if (aluno == null)
                return NotFound();

            // proteção: só edita se for Aluno
            if (!await _userManager.IsInRoleAsync(aluno, "Aluno"))
                return Forbid();

            return View(aluno);
        }

        // ✅ NOVO: Editar aluno (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> EditarAluno(ApplicationUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var aluno = await _userManager.FindByIdAsync(model.Id);
            if (aluno == null)
                return NotFound();

            if (!await _userManager.IsInRoleAsync(aluno, "Aluno"))
                return Forbid();

            // Atualiza campos permitidos
            aluno.NomeCompleto = model.NomeCompleto;
            aluno.Idade = model.Idade;
            aluno.Objetivo = model.Objetivo;

            // Email pode ser editável (se quiser travar, basta remover isso)
            var novoEmail = model.Email?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(novoEmail) && novoEmail != aluno.Email)
            {
                var exists = await _userManager.FindByEmailAsync(novoEmail);
                if (exists != null && exists.Id != aluno.Id)
                {
                    ModelState.AddModelError(nameof(model.Email), "Já existe um usuário com este e-mail.");
                    return View(model);
                }

                aluno.Email = novoEmail;
                aluno.UserName = novoEmail;
            }

            var update = await _userManager.UpdateAsync(aluno);
            if (!update.Succeeded)
            {
                foreach (var error in update.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            TempData["Sucesso"] = "Aluno atualizado com sucesso!";
            return RedirectToAction(nameof(Alunos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> ExcluirAluno(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(Alunos));

            var aluno = await _userManager.FindByIdAsync(id);
            if (aluno == null)
                return RedirectToAction(nameof(Alunos));

            if (!await _userManager.IsInRoleAsync(aluno, "Aluno"))
                return Forbid();

            // opcional: impedir excluir o próprio usuário logado (se for aluno por algum motivo)
            var userLogado = await _userManager.GetUserAsync(User);
            if (userLogado != null && aluno.Id == userLogado.Id)
            {
                TempData["Erro"] = "Você não pode excluir o próprio usuário logado.";
                return RedirectToAction(nameof(Alunos));
            }

            var result = await _userManager.DeleteAsync(aluno);

            TempData["Sucesso"] = result.Succeeded
                ? "Aluno excluído com sucesso!"
                : "Não foi possível excluir o aluno.";

            return RedirectToAction(nameof(Alunos));
        }

        // ===================== TREINOS (INSTRUTOR) =====================

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> Treinos()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
            ViewBag.Alunos = alunos.ToList();

            var treinos = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id)
                .Include(t => t.Aluno)
                .ToListAsync();

            return View(treinos);
        }

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> CriarTreino(string? alunoId = null)
        {
            var instrutor = await _userManager.GetUserAsync(User);
            if (instrutor == null)
                return Challenge();

            var alunos = await _context.Users
                .Where(u => u.TipoUsuario == "Aluno" && u.InstrutorId == instrutor.Id)
                .OrderBy(u => u.NomeCompleto)
                .ToListAsync();

            var viewModel = new CriarTreinoViewModel
            {
                Alunos = alunos,
                AlunoId = alunoId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Instrutor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarTreino(CriarTreinoViewModel model)
        {
            var instrutor = await _userManager.GetUserAsync(User);
            if (instrutor == null)
                return Challenge();

            var aluno = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Id == model.AlunoId &&
                    u.TipoUsuario == "Aluno" &&
                    u.InstrutorId == instrutor.Id);

            if (aluno == null)
            {
                ModelState.AddModelError(nameof(model.AlunoId), "Aluno inválido para este instrutor.");
            }

            if (!ModelState.IsValid)
            {
                model.Alunos = await _context.Users
                    .Where(u => u.TipoUsuario == "Aluno" && u.InstrutorId == instrutor.Id)
                    .OrderBy(u => u.NomeCompleto)
                    .ToListAsync();

                return View(model);
            }

            var treino = new Treino
            {
                Nome = model.NomeExercicio,
                DiaSemana = model.DiaSemana,
                AlunoId = model.AlunoId,
                InstrutorId = instrutor.Id,
                Concluido = false
            };

            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            return RedirectToAction("Instrutor", "Dashboard");
        }

        // ===================== INSTRUTORES (ADMIN) =====================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Instrutores()
        {
            var instrutores = await _userManager.GetUsersInRoleAsync("Instrutor");
            return View(instrutores.OrderBy(x => x.Email).ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CriarInstrutor()
        {
            return View(new CriarInstrutorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CriarInstrutor(CriarInstrutorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = model.Email.Trim().ToLower();

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Já existe um usuário com este e-mail.");
                return View(model);
            }

            var novoInstrutor = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NomeCompleto = string.IsNullOrWhiteSpace(model.NomeCompleto) ? null : model.NomeCompleto.Trim(),
                TipoUsuario = "Instrutor",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(novoInstrutor, model.Senha);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(novoInstrutor, "Instrutor");
                TempData["Sucesso"] = "Instrutor criado com sucesso!";
                return RedirectToAction(nameof(Instrutores));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluirInstrutor(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(Instrutores));

            var instrutor = await _userManager.FindByIdAsync(id);
            if (instrutor == null)
                return RedirectToAction(nameof(Instrutores));

            var userLogado = await _userManager.GetUserAsync(User);
            if (userLogado != null && instrutor.Id == userLogado.Id)
            {
                TempData["Erro"] = "Você não pode excluir o próprio usuário logado.";
                return RedirectToAction(nameof(Instrutores));
            }

            if (await _userManager.IsInRoleAsync(instrutor, "Admin"))
            {
                TempData["Erro"] = "Não é permitido excluir um usuário Admin.";
                return RedirectToAction(nameof(Instrutores));
            }

            var result = await _userManager.DeleteAsync(instrutor);

            TempData["Sucesso"] = result.Succeeded
                ? "Instrutor excluído com sucesso!"
                : "Não foi possível excluir o instrutor.";

            return RedirectToAction(nameof(Instrutores));
        }
    }
}