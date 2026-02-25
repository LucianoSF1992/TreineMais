using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TreineMais.Models;

namespace TreineMais.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user?.TipoUsuario == "Instrutor")
                    return RedirectToAction("Index", "Admin");

                if (user?.TipoUsuario == "Aluno")
                    return RedirectToAction("Index", "Aluno");
            }

            return View();
        }
    }
}