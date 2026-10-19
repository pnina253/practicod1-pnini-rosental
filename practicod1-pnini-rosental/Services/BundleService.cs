
using System.Text;



namespace practicod1_pnini_rosental.Services
{
    public class BundleService
    {
        public void CreateBundle(List<string> files, string outputPath, string sort,
            bool note,
            bool removeEmptyLines,
            string? author, string sourceDirectory)
        {
            IEnumerable<string> sortedFiles = files;

            if (sort.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                sortedFiles = files.OrderBy(
                    file => Path.GetFileName(file),
                    StringComparer.OrdinalIgnoreCase);
            }
            else if (sort.Equals("language", StringComparison.OrdinalIgnoreCase))
            {
                sortedFiles = files
                    .OrderBy(file => Path.GetExtension(file),
                        StringComparer.OrdinalIgnoreCase)
                    .ThenBy(file => Path.GetFileName(file),
                        StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                throw new ArgumentException(
                    "Sort must be 'name' or 'language'.");
            }

            string fullOutputPath = Path.GetFullPath(outputPath);
            string? outputDirectory = Path.GetDirectoryName(fullOutputPath);

            if (!string.IsNullOrEmpty(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            using var writer = new StreamWriter(
                fullOutputPath, false, Encoding.UTF8);

            if (!string.IsNullOrWhiteSpace(author))
            {
                writer.WriteLine($"// Author: {author}");
                writer.WriteLine();
            }

            foreach (string file in sortedFiles)
            {
                if (string.Equals(
                Path.GetFullPath(file),
                fullOutputPath,
                StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                string content = System.IO.File.ReadAllText(file);
                if (removeEmptyLines)
                {
                    content = string.Join(
                        Environment.NewLine,
                        content.Split(
                            new[] { "\r\n", "\n", "\r" },
                            StringSplitOptions.None)
                        .Where(line => !string.IsNullOrWhiteSpace(line)));
                }

                if (note)
                {
                    string fileName = Path.GetFileName(file);
                    string relativePath = Path.GetRelativePath(
                        sourceDirectory, file);

                    writer.WriteLine($"// File: {fileName}");
                    writer.WriteLine($"// Path: {relativePath}");
                    writer.WriteLine();
                }
                writer.WriteLine(content);
                writer.WriteLine();
            }

        }
    }

}
