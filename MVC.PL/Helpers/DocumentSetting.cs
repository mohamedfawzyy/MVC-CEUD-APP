using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
namespace MVC.PL.Helpers
{
    public static class DocumentSetting
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string FolderName) {

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            
            if (!Directory.Exists(FolderPath)) {
                Directory.CreateDirectory(FolderPath);
            }
            string fileName =$"{Guid.NewGuid()}{file.FileName}";

            string filePath=Path.Combine(FolderPath,fileName);

           using var FileStrame = new FileStream(filePath, FileMode.Create);

           await file.CopyToAsync(FileStrame);

            return fileName;
                
        }

        public static void DeleteFile(string FolderName , string FileName)
        {
            string FilePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName,FileName);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
