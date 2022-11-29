using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace TypicalTools.Services
{
    public class FileLoaderService
    {
        string uploadPath;
        EncryptionService encryptionService;

        public FileLoaderService(IWebHostEnvironment env, EncryptionService encryption)
        {
            //path in wwwroot
            uploadPath = Path.Combine(env.WebRootPath, "Uploads");
            encryptionService = encryption;
        }

        /// <summary>
        /// saving method for the warrranties
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task SaveFile(IFormFile file)
        {
            //Possibly check for unique file name

            byte[] fileContents;


            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                fileContents = stream.ToArray();
            }

            byte[] encryptedContents = encryptionService.EncryptByteArray(fileContents);

            using (MemoryStream dataStream = new MemoryStream(encryptedContents))
            {
                string targetFile = Path.Combine(uploadPath, file.FileName);

                using (FileStream fileStream = new FileStream(targetFile, FileMode.Create))
                {
                    dataStream.WriteTo(fileStream);
                }
            }
        }

        public async Task<FileInfo> LoadFile(string fileName)
        {
            DirectoryInfo directory = new DirectoryInfo(uploadPath);

            if (directory.EnumerateFiles().Any(c => c.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase)) == false)
            {
                return null;
            }

            return directory.EnumerateFiles().Where(c => c.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public async Task<byte[]> ReadFileIntoMemory(string fileName)
        {
            var file = await LoadFile(fileName);

            if (file == null)
            {
                return null;
            }

            using (var memStream = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(file.FullName))
                {
                    fileStream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
        }

        public async Task<byte[]> LoadEncryptedFile(string fileName)
        {
            var encryptedFile = await ReadFileIntoMemory(fileName);

            if (encryptedFile == null || encryptedFile.Length == 0)
            {
                return null;
            }
            var decryptedData = encryptionService.DecryptByteArray(encryptedFile);

            return decryptedData;
        }
    }
}
