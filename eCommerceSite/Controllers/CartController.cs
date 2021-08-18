using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        /// <summary>
        /// Adds a product to the shopping cart
        /// </summary>
        /// <param name="id">The id of the product to add</param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id)
        {
            // Get the product from the database
            Product p = await ProductDB.getProductAsync(_context, id);

            CookieHelper.addProductToCart(_httpContext, p);

            TempData["Message"] = $"{p.Title} added successfully";

            // Redirect back to previous page
            return RedirectToAction("Index","Product");
        }

        public IActionResult Summary()
        {
            return View(CookieHelper.getCartProducts(_httpContext));
        }
    }
}
