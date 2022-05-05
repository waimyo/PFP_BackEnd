using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IFileSystemService
    {
        bool SaveFile(IFormFile file, string filepath, string filename);
        byte[] GetFile(string FilePath, string FileName);
        bool RemoveFile(string filePath);
    }
}
