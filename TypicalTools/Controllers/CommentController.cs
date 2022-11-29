using TypicalTools.DataAccess;
using TypicalTools.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TypicalTools.Controllers
{
    public class CommentController : Controller
    {
        private readonly DapperContext context;

        public CommentController(DapperContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// receives all the comment list for a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>index</returns>
        [HttpGet]
        public async Task<IActionResult> CommentList(int id)
        {
            List<Comment> comments = await context.GetCommentsForProductAsync(id);

            if(comments == null)
            {
                return RedirectToAction("Index", "Product");
            }

            return View(comments);
        }


        // Show a form to add a new comment
        [HttpGet]
        public async Task<IActionResult> AddComment(int productCode)
        {
            Comment comment = new Comment();
            comment.ProductCode = productCode;
            return View(comment);
        }

        // Receive and handle the newly created comment data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            await context.AddComment(comment, HttpContext.Session.Id);

            // A session id is only set once a value has been added!
            // adding a value here to ensure the session is created
            HttpContext.Session.SetString("CommentText", comment.CommentText);

            return RedirectToAction("Index", "Product");
        }

        // Receive and handle a request to Delete a comment
        public async Task<IActionResult> RemoveComment(int commentId)
        {
            var comment = await context.GetSingleComment(commentId);

            // Check if the admin is logged in
            string authStatus = HttpContext.Session.GetString("Role");
            bool isAdmin = !String.IsNullOrWhiteSpace(authStatus);

            // Peform the deletion conditionally
            if (comment.SessionId == HttpContext.Session.Id || isAdmin)
            {
                await context.DeleteComment(commentId);
            }

            return RedirectToAction("CommentList", "Comment", new { id = comment.ProductCode });
        }

        // Show a existing comment details in a form to allow for editing
        [HttpGet]
        public async Task<IActionResult> EditComment(int commentId)
        {
            Comment comment = await context.GetSingleComment(commentId);
            return View(comment);
        }

        //Receive and handle the edited comment data
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(Comment comment)
        {
            if (comment == null)
            {
                return RedirectToAction("Index", "Product");
            }

            // Check if the admin is logged in
            string authStatus = HttpContext.Session.GetString("Role");
            bool isAdmin = !String.IsNullOrWhiteSpace(authStatus);

            if (comment.SessionId == HttpContext.Session.Id || isAdmin)
            {
                await context.EditComment(comment);
            }
            return RedirectToAction("CommentList", "Comment", new { id = comment.ProductCode });

        }
    }
}
