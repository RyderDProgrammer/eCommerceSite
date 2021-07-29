﻿using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Displays a view that lists all products.
        /// </summary>
        public IActionResult Index()
        {
            //Get all products from database
            //List<Product> products = _context.Products.ToList();
            List<Product> products =
                (from p in _context.Products
                 select p).ToList();

            //Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product p)
        {
            if (ModelState.IsValid)
            {
                //Add to the database
                _context.Products.Add(p);
                //Make sure it saves what its done.
                _context.SaveChanges();

                TempData["Message"] = $"{p.Title} was added successfully!";

                //redirect back to catalog page.
                return RedirectToAction("Index");
            }


            return View();
        }
    }
}
