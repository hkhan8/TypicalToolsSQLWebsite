using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TypicalTools.Models;

namespace TypicalTools.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LoginIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginIndex(Login user)
        {
            //if (user.Password.Equals("SecurePassword123"))
            //{
            //    HttpContext.Session.SetString("Authenticated", "True");
            //}
            return RedirectToAction("Index", "Product");
        }
    }
}
