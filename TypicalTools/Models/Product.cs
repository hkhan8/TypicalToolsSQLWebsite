using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TypicalTools.Models
{
    public class Product
    {
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        //validation for product price
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Enter value with 2 decimals (eg. 45.00, 45.99)")]
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
