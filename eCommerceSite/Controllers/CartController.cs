﻿using eCommerceSite.Data;
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

            const string CartCookie = "CartCookie";

            //Get existing cart items
            string readCookie = _httpContext.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();
            if(readCookie != null)
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(readCookie);
            }

            //Add current product to existing cart
            cartProducts.Add(p);

            // Add the products to cart cookie
            //string data = JsonConvert.SerializeObject(p);
            string data = JsonConvert.SerializeObject(cartProducts);
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1),
                Secure = true,
                IsEssential = true
            };

            _httpContext.HttpContext.Response.Cookies.Append(CartCookie, data, options);

            // Redirect back to previous page
            return RedirectToAction("Index","Product");
        }

        public IActionResult Summary()
        {
            const string CartCookie = "CartCookie";
            string cookieData = _httpContext.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();
            if (cookieData != null)
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(cookieData);
            }

            return View(cartProducts);
        }
    }
}
