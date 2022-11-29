using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using TypicalTools.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TypicalTools.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;

namespace TypicalTools.DataAccess
{
    public class DapperContext
    {
        //Variable to hold the configuration properties object
        private readonly IConfiguration config;


        /// <summary>
        /// Constructor to setup the class by storing the config properties object passed by the constructor.
        /// As this application uses Dependency Injection, this bject is passed automatically by the Services component of the project.
        /// </summary>
        /// <param name="configuration">The config properites object provided by dependency injection</param>
        public DapperContext(IConfiguration configuration)
        {
            config = configuration;
        }

        /// <summary>
        /// Retrieves all team records from the database.
        /// </summary>
        /// <returns>The Team records in a list format.</returns>
        public async Task<List<Product>> ParseProducts()
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                string sql = "SELECT * FROM products;";
                List<Product> productList = new List<Product>();

                var productTable = await connection.ExecuteReaderAsync(sql);

                while (productTable.Read())
                {
                    //instantiation
                    Product productList_ = new Product();
                    //adding properting to productList
                    productList_.ProductCode = int.Parse(productTable.GetValue(0).ToString());
                    productList_.ProductName = productTable.GetValue(1).ToString();
                    productList_.ProductPrice = decimal.Parse(productTable.GetValue(2).ToString());
                    productList_.ProductDescription = productTable.GetValue(3).ToString();
                    try
                    {
                        //in try catch to stop application crash
                        string s = productTable.GetValue(4).ToString();
                        productList_.UpdatedDate = DateTime.Parse(s);
                    }
                    catch (Exception e)
                    {

                        //throw;
                    }
                    try
                    {
                        string x = productTable.GetValue(5).ToString();
                        productList_.IsNew = bool.Parse(productTable.GetValue(5).ToString());
                    }
                    catch (Exception e)
                    {
                        //throw;
                    }                    
                    productList.Add(productList_);
                }
                return productList;
            }
        }
        /// <summary>
        /// gets single products by product code
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns>list of single product</returns>
        public async Task<Product> GetSingleProduct(int productCode)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                var productList_ = await ParseProducts();
                return productList_.Where(c => c.ProductCode == productCode).FirstOrDefault();
            }
        }
        /// <summary>
        /// retrieves all comments from database
        /// </summary>
        /// <returns>list of comments</returns>
        public async Task<List<Comment>> ParseComments()
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                string sql = "SELECT * FROM comments;";
                List<Comment> commentList = new List<Comment>();

                var commentTable = await connection.ExecuteReaderAsync(sql);

                while (commentTable.Read())
                {
                    //instantiation of commentList
                    Comment commentList_ = new Comment();
                    commentList_.CommentId = int.Parse(commentTable.GetValue(0).ToString());
                    commentList_.CommentText = commentTable.GetValue(1).ToString();
                    commentList_.ProductCode = int.Parse(commentTable.GetValue(2).ToString());
                    commentList_.SessionId = commentTable.GetValue(3).ToString();
                    try
                    {
                        string s = commentTable.GetValue(4).ToString();
                        commentList_.CreatedDate = DateTime.Parse(s);
                    }
                    catch (Exception e)
                    {

                        //throw;
                    }
                    
                    commentList.Add(commentList_);
                }
                return commentList;
            }
        }
        /// <summary>
        /// retrieve comments for specific product
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns>list</returns>
        public async Task<List<Comment>> GetCommentsForProductAsync(int productCode)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                if (productCode == 0)
                {
                    return null;
                }

                var allComments = await ParseComments();

                // Return all comments where the productcode matches the provided product code
                return allComments.Where(c => c.ProductCode == productCode).ToList();
            }
        }
        /// <summary>
        /// adds comments to product in the right session
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public async Task AddComment(Comment comment, string sessionId)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                string sql = "INSERT INTO comments(comment_text, product_code, session_id)" +
                             $"VALUES('{comment.CommentText}', {comment.ProductCode}, '{sessionId}');";
                await connection.ExecuteAsync(sql);
            }
        }
        /// <summary>
        /// adds product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task AddProduct(Product product)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                string sql = "INSERT INTO products(product_name, product_price, product_description)" +
                             $"VALUES('{product.ProductName}', {product.ProductPrice}, '{product.ProductDescription}');";
                await connection.ExecuteAsync(sql);



            }
        }
        /// <summary>
        /// updating price of product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task UpdatePrice(Product product)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                string sql = "UPDATE products " +
                             $"SET product_price = {product.ProductPrice}, is_new = 0 "+
                             $"WHERE product_code = {product.ProductCode};";
                             
                await connection.ExecuteAsync(sql);
            }
        }
        /// <summary>
        /// editing specific comment
        /// </summary>
        /// <param name="updatedComment"></param>
        /// <returns>bool</returns>
        public async Task<bool> EditComment(Comment updatedComment)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                string sql = "UPDATE comments " +
                             $"SET comment_text = '{updatedComment.CommentText}' " +
                             $"WHERE comment_id = {updatedComment.CommentId}";
                await connection.ExecuteAsync(sql);
            }
            return true;
        }

        public async Task<Comment> GetSingleComment(int commentId)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                var comments = await ParseComments();
                return comments.Where(c => c.CommentId == commentId).FirstOrDefault();
            }
        }
        /// <summary>
        /// deleting a specific comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteComment(int commentId)
        {
            //Using statement to create connection object and automatically close and dispose of connection once finished.
            using (var connection = new SqlConnection(config.GetConnectionString("Default")))
            {
                var existingComments = await ParseComments();
                //loop through and delete matching comment
                foreach (var item in existingComments)
                {
                    if (item.CommentId == commentId)
                    {
                        string sql = "DELETE from comments " +
                                      $"WHERE comment_id = {commentId}";
                        await connection.ExecuteAsync(sql);

                        return true;
                    }
                }
                return false;
            }     
        }

        public AdminUser CheckLogin(AdminUser account)
        {
            try
            {
                using (var connection = new SqlConnection(config.GetConnectionString("Default")))
                {
                    string sql = "SELECT * FROM dbo.users WHERE \"UserName\" = @UserName";
                    AdminUser user = connection.QuerySingle<AdminUser>(sql, account);

                    if (user == null)
                    {
                        return null;
                    }

                    string passwordCheck = PasswordHasher.ConvertStringToHash(account.Password);
                    //if (passwordCheck.Equals(user.Password))
                    if (passwordCheck == user.Password)
                    {
                        return user;
                    }

                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        //public async Task<int> GetUserCount (string username, string password)
        //{
        //    //Using statement to create connection object and automatically close and dispose of connection once finished.
        //    using (var connection = new SqlConnection(config.GetConnectionString("Default")))
        //    {
        //        connection.Open();

        //        //parameterized query for login
        //        string sql = "SELECT count(UserName) FROM dbo.users WHERE \"UserName\" = @UserName AND \"Password\" = @Password;";
        //        SqlCommand sqlCommand = new SqlCommand(sql, connection);

        //        sqlCommand.CommandType = CommandType.Text;
        //        sqlCommand.Parameters.AddWithValue("@UserName", username);
        //        sqlCommand.Parameters.AddWithValue("@Password", password);

        //        int count = 0;
        //        try
        //        {
        //            var a = (int)sqlCommand.ExecuteScalar();
        //            count = a;
        //        }
        //        catch (Exception e)
        //        {
        //            // to not break app if error
        //        }
        //        connection.Close();
        //        return count;
        //    }
        //}

        public bool CreateAccount(AdminUser account)
        {
            try
            {
                using (var connection = new SqlConnection(config.GetConnectionString("Default")))
                {
                    string sql = "SELECT COUNT(*) FROM dbo.users WHERE UserName = @UserName";
                    int count = connection.QuerySingle<int>(sql, account);

                    if (count > 0)
                    {
                        return false;
                    }

                    account.Password = PasswordHasher.ConvertStringToHash(account.Password);

                    sql = "INSERT INTO dbo.users (UserName,Password, Role) " +
                          "VALUES (@UserName, @Password, @Role)";
                    connection.Execute(sql, account);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}