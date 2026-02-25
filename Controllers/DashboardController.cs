using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TreineMais.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
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