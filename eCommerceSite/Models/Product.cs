using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// A salable product
    /// </summary>
    public class Product
    {
        [Key] //Make primary key in database
        public int ProductId { get; set; }

        /// <summary>
        /// The consumer facing name of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The retail price of the product in US dollars
        /// </summary>
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        /// <summary>
        /// Category product falls under. Ex. electronics, furniture, etc.
        /// </summary>
        public string Category { get; set; }
    }
}
