using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if(ModelState.IsValid)
            {
                //Map data to user account instance/object
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    UserName = reg.Username
                };
                //add to DB
                _context.UserAccounts.Add(acc);
                await _context.SaveChangesAsync();

                //redirect to home page
                return RedirectToAction("Index", "Home");
            }
            return View(reg);
        }
    }
}
