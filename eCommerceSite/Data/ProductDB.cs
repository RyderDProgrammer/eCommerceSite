using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{
    static public class ProductDB
    {
        /// <summary>
        /// Returns the total count of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        public async static Task<int> getTotalProductsAsync(ProductContext _context)
        {
            return await(from p in _context.Products select p).CountAsync();
        }

        /// <summary>
        /// Get a page worth of products
        /// </summary>
        /// <param name="_context">The database context</param>
        /// <param name="pageSize">Number of products per page</param>
        /// <param name="pageNum">Page of products to return</param>
        public async static Task<List<Product>> getProductsAsync(ProductContext _context,int pageSize, int pageNum)
        {
            return  await (from p in _context.Products
                    orderby p.Title ascending
                    select p).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToListAsync(); //Skip must be before take.
        }

        public async static Task<Product> addProductAsync(ProductContext _context, Product p)
        {
            //Add to the database
            _context.Products.Add(p);
            //Make sure it saves what its done.
            await _context.SaveChangesAsync();
            return p;
        }
    }
}
