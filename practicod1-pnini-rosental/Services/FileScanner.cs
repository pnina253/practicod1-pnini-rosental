using practicod1_pnini_rosental.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace practicod1_pnini_rosental.Services
{
    public class FileScanner
    {
        private readonly HashSet<string> excludedDirectories =
        [
            "bin",
            "obj",
            "debug",
            "release",
            ".git"
        ];

        public List<string> ScanFiles(string directoryPath, string language)
        {
            var extensions = language.GetLanguage();

            if (extensions == null)
            {
                throw new ArgumentException("Unknown programming language.");
            }

            var files = new List<string>();

            ScanDirectory(directoryPath, extensions, files);

            return files;
        }

        private void ScanDirectory(
            string directoryPath,
            string[] extensions,
            List<string> files)
        {
            foreach (string file in Directory.EnumerateFiles(directoryPath))
            {
                if (extensions.Contains(Path.GetExtension(file),
                    StringComparer.OrdinalIgnoreCase))
                {
                    files.Add(file);
                }
            }

            foreach (string directory in Directory.EnumerateDirectories(directoryPath))
            {
                string directoryName = Path.GetFileName(directory);

                if (excludedDirectories.Contains(directoryName,
                    StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                ScanDirectory(directory, extensions, files);
            }
        }
    }
}