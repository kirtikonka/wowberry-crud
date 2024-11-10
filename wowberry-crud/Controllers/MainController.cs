using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wowberry_crud.Data;
using wowberry_crud.Models;

namespace wowberry_crud.Controllers
{
    public class MainController : Controller
    {
        // Inserting Private Fields
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment env;

        // Parameterised Constructor
        public MainController(ApplicationDbContext db, IWebHostEnvironment env) 
        {
            this.db = db;
            this.env = env;
        }

        // SignIn
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(Users log)
        {
            var data = db.users.Where(x => x.UserName.Equals(log.UserName) && x.Password.Equals(log.Password)).SingleOrDefault();
            if (data != null)
            {
                bool us = data.UserName.Equals(log.UserName) && data.Password.Equals(log.Password);
                if (us)
                {
                    //step 3 passing cookies
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, log.UserName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("Username", log.UserName);

                    return RedirectToAction("Index", "Entries");
                }
                else
                {
                    TempData["Pass"] = "Invalid Password";
                }
            }
            else
            {
                TempData["us"] = "Username invalid";
            }
            return View();

        }

        // SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(Users u)
        {
            u.Password = u.Password;
            db.users.Add(u);
            db.SaveChanges();
            return RedirectToAction("SignIn");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedcookies = Request.Cookies.Keys;
            foreach (var cookie in storedcookies)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("SignIn");
        }
    }
}
