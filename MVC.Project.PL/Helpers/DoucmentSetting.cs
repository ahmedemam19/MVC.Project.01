using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MVC.Project.PL.Helpers
{
    public static class DoucmentSetting
    {

        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Loacted Folder Path

            //string folderpath = $"D:\\.NET\\Assignment\\08 MVC\\MVC-05\\MVC.Project.01\\MVC.Project.PL\\wwwroot\\files\\{folderName}";
            //string folderpath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{folderName}";
            string folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            if(Directory.Exists(folderpath))
                Directory.CreateDirectory(folderpath); /// to check that the previous path is valid 
                                                       /// and if not it will Create the required folders

            // 2. Get File Name and make it [ UNIQUE ]
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";


            // 3. Get File Path 
            string filePath = Path.Combine(folderpath, fileName);


            // 4. Save file as streams [ Data per time ]
            using var fileStream = new FileStream(filePath, FileMode.Create);


            await file.CopyToAsync(fileStream);
            // the contnet of [ file ] is copied to [ fileStream ] on Connection [ filePath ] to Ctreate using [ FileMode.Create ]


            return fileName;

        }


        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);

        }

    }
}
