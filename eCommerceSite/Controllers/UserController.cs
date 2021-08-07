using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Login()
        {
            //Check if user is already logged into website
            if(HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel log)
        {
            if(!ModelState.IsValid)
            {
                return View(log);
            }
            UserAccount acc =
                await (from u in _context.UserAccounts
                 where (u.UserName == log.UsernameOrEmail || u.Email == log.UsernameOrEmail) && u.Password == log.Password
                 select u).SingleOrDefaultAsync();
            /* 
            Another way of doing it in method syntax
            UserAccount account = 
                _context.UserAccounts.Where(userAcc => userAcc.UserName == log.userNameOrEmail || userAcc.email == log.userNameOrEmail && userAcc.Password == log.Password).SingleOrDefaultAsync();
            */
            if(acc == null)
            {
                //Custom error message
                ModelState.AddModelError(string.Empty, "Credentials were not found");
                return View(log);
            }
            //Log user into website
            HttpContext.Session.SetInt32("UserId",acc.UserId);

            return RedirectToAction("Index", "Home");
        }
    }
}
