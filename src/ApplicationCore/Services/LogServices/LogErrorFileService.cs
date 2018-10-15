using System;
using System.IO;
using System.Text;

namespace ApplicationCore.Services.LogServices
{
    public class LogErrorFileService : ILogErrorService
    {
        public void Notify(Exception exception, string username, string urlPath)
        {
            var now = DateTime.Now.ToString("dd-MM-yyyy");
            var filename = $"LogError-{now}.txt";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, filename);
            var fileStream = new FileStream(filePath, FileMode.Append);
            var content = new StringBuilder();

            content.Append($"Path = \"{urlPath}\"");
            content.Append($"\nUsername = \"{username}\"");
            content.Append($"\nMessage = \"{exception.Message}\"");
            content.Append($"\nFecha = {DateTime.Now}");
            content.Append($"\n\n\n{exception.StackTrace}");
            content.Append("\n\n=====================================================================================");
            content.Append("\n=====================================================================================");

            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine(content.ToString());
            }
        }
    }
}
