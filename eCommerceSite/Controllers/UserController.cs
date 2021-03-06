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
                //Check if username/email is in use
                bool isEmailTaken = await (from account in _context.UserAccounts
                                    where account.Email == reg.Email
                                    select account).AnyAsync();

                //if so, add custom error message and send back to view
                if(isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "Email is already in use");
                }

                bool isUserNameTaken = await(from account in _context.UserAccounts
                                       where account.UserName == reg.Username
                                       select account).AnyAsync();
                if(isUserNameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Username), "Username is already in use");
                }

                if(isEmailTaken || isUserNameTaken)
                {
                    return View(reg);
                }


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

                logUserIn(acc.UserId);

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
            if (!ModelState.IsValid)
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
            if (acc == null)
            {
                //Custom error message
                ModelState.AddModelError(string.Empty, "Credentials were not found");
                return View(log);
            }
            logUserIn(acc.UserId);

            return RedirectToAction("Index", "Home");
        }

        private void logUserIn(int accountId)
        {
            //Log user into website
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            //Removes all current session data.
            HttpContext.Session.Clear();
            return RedirectToAction(actionName:"Index", controllerName:"Home");
        }
    }
}
