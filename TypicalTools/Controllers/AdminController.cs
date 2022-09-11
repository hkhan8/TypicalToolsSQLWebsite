using TypicalTools.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TypicalTools.DataAccess;
using System.Threading.Tasks;

namespace TypicalTools.Controllers
{
    public class AdminController : Controller
    {
        private readonly DapperContext context;

        public AdminController(DapperContext context)
        {
            this.context = context;
        }
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            var y = await context.GetUserDetails(username, password);

            if (y > 0)
            {
                HttpContext.Session.SetString("Authenticated", "True");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                HttpContext.Session.SetString("Not-Authenticated", "False");
                ModelState.AddModelError("", "Invalid User Login");
            }
            return View();
        }
    }
}
