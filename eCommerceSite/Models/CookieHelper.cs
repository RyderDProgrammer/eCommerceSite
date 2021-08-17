using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public static class CookieHelper
    {
        const string CartCookie = "CartCookie";
        /// <summary>
        /// Returns the current list of cart products. If cart is empty an empty list
        /// will be returned.
        /// </summary>
        public static List<Product> getCartProducts(IHttpContextAccessor http)
        {
            //Get existing cart items
            string readCookie = http.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();
            if (readCookie != null)
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(readCookie);
            }

            return cartProducts;
        }

        /// <summary>
        /// Allows the user to add to the cart and makes use of getting the cart products above.
        /// </summary>
        /// <param name="p"></param>
        public static void addProductToCart(IHttpContextAccessor http, Product p)
        {
            List<Product> cartProducts = getCartProducts(http);
            cartProducts.Add(p);

            string data = JsonConvert.SerializeObject(cartProducts);

            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1),
                Secure = true,
                IsEssential = true
            };

            http.HttpContext.Response.Cookies.Append(CartCookie,data,options);
        }

        /// <summary>
        /// Just a simple number that displays the amount of items in the cart.
        /// </summary>
        /// <returns></returns>
        public static int getTotalCartProducts(IHttpContextAccessor http)
        {
            List<Product> cartProducts = getCartProducts(http);
            return cartProducts.Count;
        }
    }
}
