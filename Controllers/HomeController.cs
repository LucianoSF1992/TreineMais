using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                // (Opcional) garante usuário existe
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // ✅ centraliza a regra de redirecionamento no DashboardController.Index()
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return View();
        }
    }
}