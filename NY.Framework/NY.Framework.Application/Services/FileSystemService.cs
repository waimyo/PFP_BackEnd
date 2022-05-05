using Microsoft.AspNetCore.Http;
using NY.Framework.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class FileSystemService : IFileSystemService
    {
        public byte[] GetFile(string FilePath, string FileName)
        {
            byte[] filebyte = null;
            var path = Path.Combine(FilePath, Path.GetFileName(FileName));
            if (System.IO.File.Exists(path))
            {
                filebyte = System.IO.File.ReadAllBytes(path);
            }
            return filebyte;
        }

        public bool RemoveFile(string filePath)
        {
            bool remove = false;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                remove = true;
            }
            return remove;
        }

        public bool SaveFile(IFormFile file, string filepath, string filename)
        {
            bool saved = true;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            // original 
            var path = Path.Combine(filepath, filename);
            try
            {
                using (FileStream fs = System.IO.File.Create(path))
                {
                    file.CopyTo(fs);
                }
            }
            catch(Exception ex)
            {
                
            }
            return saved;
        }
    }
}
