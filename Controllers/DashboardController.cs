using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TreineMais.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // ✅ Rota padrão /Dashboard -> manda para a tela certa conforme a Role
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction(nameof(Admin));

            if (User.IsInRole("Instrutor"))
                return RedirectToAction(nameof(Instrutor));

            if (User.IsInRole("Aluno"))
                return RedirectToAction(nameof(Aluno));

            // fallback
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Instrutor")]
        public IActionResult Instrutor()
        {
            return View();
        }

        [Authorize(Roles = "Aluno")]
        public IActionResult Aluno()
        {
            return View();
        }
    }
}