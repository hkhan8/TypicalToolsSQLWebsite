using TypicalTools.DataAccess;
using TypicalTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Web;

namespace TypicalTools.Controllers
{
    public class ProductController : Controller
    {
        private readonly DapperContext context;

        public ProductController(DapperContext context)
        {
            this.context = context;
        }

        // Show all products
        public async Task<IActionResult> Index()
        {
            var products = await context.ParseProducts();
            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View(products.ToList());
        }

        // Show a form to add a new product
        [HttpGet]
        public async Task<IActionResult> AddProduct(string productName)
        {
            Product product = new Product();
            product.ProductName = productName;
            return View(product);
        }

        // Receive and handle the newly created product data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // Check if the admin is logged in
            string authStatus = HttpContext.Session.GetString("Role");
            bool isAdmin = !String.IsNullOrWhiteSpace(authStatus);

            //encoding product description to prevent script injection attack
            StringBuilder sb = new StringBuilder();
            sb.Append(HttpUtility.HtmlEncode(product.ProductDescription));

            product.ProductDescription = sb.ToString();

            string strDes = HttpUtility.HtmlEncode(product.ProductDescription);
            product.ProductDescription = strDes;


            // Peform conditionally
            if (product.SessionId == HttpContext.Session.Id || isAdmin)
            {
                await context.AddProduct(product);
            }

            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            // A session id is only set once a value has been added!
            // adding a value here to ensure the session is created
            HttpContext.Session.SetString("ProductName", product.ProductName);
            // string to return in a currency format
            HttpContext.Session.SetString("ProductPrice", product.ProductPrice.ToString());
            HttpContext.Session.SetString("ProductDescription", product.ProductDescription);

            return RedirectToAction("Index", "Product");
        }

        // Show a form to add a new product
        [HttpGet]
        public async Task<IActionResult> UpdatePrice(int id)
        {
            Product product = await context.GetSingleProduct(id);
            return View(product);
        }

        // Receive and handle the newly updated price data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrice(Product product)
        {
            await context.UpdatePrice(product);

            // A session id is only set once a value has been added!
            // adding a value here to ensure the session is created
            // string to return in a currency format
            HttpContext.Session.SetString("ProductPrice", product.ProductPrice.ToString());

            return RedirectToAction("Index", "Product");
        }

    }

}

