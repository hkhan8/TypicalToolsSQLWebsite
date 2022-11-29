using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using TypicalTools.DataAccess;
using TypicalTools.Models;
using TypicalTools.Services;

namespace TypicalTools.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly FileLoaderService _loader;

        //constructor
        public UploadController(ILogger<UploadController> logger, FileLoaderService loader)
        {
            _logger = logger;
            _loader = loader;
        }

        /// <summary>
        /// main page of upload/download
        /// </summary>
        /// <returns>index</returns>
        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");
            return View(nameof(Index), role);
        }

        /// <summary>
        /// allows user to upload the warranty file for his products
        /// </summary>
        /// <param name="file"></param>
        /// <returns>file upload</returns>
        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            //no file is selected
            if (file == null)
            {
                return NoContent();
            }                       
            else
            {
                //save to database method
                await _loader.SaveFile(file); 
            }
            ViewBag.Message = "File has been uploaded successfully";
            return View("Index");
        }

        /// <summary>
        /// download option for admin to receive uploaded warranty files
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>downloaded file</returns>
        [HttpPost]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            //decryption of file
            byte[] fileBytes = await _loader.LoadEncryptedFile(filename);

            //validation checks on file
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            //saves file to host storage
            return File(fileBytes, "application/octet-stream", filename);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
