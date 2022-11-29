using TypicalTools.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TypicalTools.DataAccess;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace TypicalTools.Controllers
{
    public class AdminController : Controller
    {
        //sql queries of methods
        private readonly DapperContext context;

        public AdminController(DapperContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// main form of user login
        /// </summary>
        /// <returns>index</returns>
        public IActionResult AdminLogin()
        {
            return View();
        }

        /// <summary>
        /// check login of user
        /// </summary>
        /// <param name="account"></param>
        /// <returns>view of account if logged in</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(AdminUser account)
        {
            if (ModelState.IsValid)
            {
                //The application will check the login status of the entered values with the database
                //and confirm that it is infact a valid login attempt. If it passes the validation check,
                //the program will proceed to assign a role to the user for authorisation purposes. 
                //If authentication is not valid, then only product page is visible and ability to read comments.
                AdminUser logged = context.CheckLogin(account);
                if (logged != null)
                { 
                    //If the authentication is granted to a specifc user, a role will be set in the HttpSession data
                    //to keep the user logged in. There are 2 types of roles in this application(Admin & User) and both
                    //have different accessibility in the website. For eg. Admin can create user with a role set or 
                    //admins can download warranties. 
                    HttpContext.Session.SetString("Role", logged.Role);
                    return RedirectToAction("Index", "Product");
                }
            }
            ModelState.AddModelError("", "Invalid User Login or Pass");
            return View(account);
        }

        /// <summary>
        /// logs user out and clears session
        /// </summary>
        /// <returns>index page</returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Register()
        {
            //gets role value of the session needed for view
            ViewBag.Role = HttpContext.Session.GetString("Role");
            PopulateRoleOptions();
            return View();
        }

        /// <summary>
        /// populates roles from enum
        /// </summary>
        private void PopulateRoleOptions()
        {
            var roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = ((int)c).ToString()
            });
            ViewBag.roles = roles;
        }

        /// <summary>
        /// register a new account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AdminUser account)
        {
            //validation checks on password
            if (account.Password.Equals(account.Password) == false)
            {
                ModelState.AddModelError("", "Make sure passwords match");
                PopulateRoleOptions();
                return View(account);
            }            
            //receive string of role from http session
            var role = HttpContext.Session.GetString("Role");

            if (role != null && role == "Admin")
            {
                account.Role = Enum.GetValues(typeof(Roles)).Cast<Roles>().ElementAt(int.Parse(account.Role)).ToString();
            }
            else
            {
                account.Role = Roles.User.ToString();
            }
            //create new account in database
            bool status = context.CreateAccount(account);

            if (status)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            //user is already found in database
            PopulateRoleOptions();
            ModelState.AddModelError("", "User exists already");
            return View(account);
        }

        public enum Roles
        {
            //role options for admin
            Admin,
            Moderator,
            User
        }
    }
}
