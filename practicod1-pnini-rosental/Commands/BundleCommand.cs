using practicod1_pnini_rosental.Helpers;
using practicod1_pnini_rosental.Services;
using System.CommandLine;

namespace practicod1_pnini_rosental.Commands
{
    public class BundleCommand
    {
        public static Command Create()
        {
            var bundleCommand = new Command("bundle", "Bundle source files");


            //language

            var languageOption = new Option<string>("--language")
            {
                Description = "The programming language",
                Required = true
            };


            languageOption.Aliases.Add("-l");


            //output
            var outputOption = new Option<string>("--output")
            {
                Description = "The output file path"
            };

            outputOption.Aliases.Add("-o");

            //note
            var noteOption = new Option<bool>("--note")
            { Description = "Adding notes to the file" };

            noteOption.Aliases.Add("-n");

            //sort
            var sortOption = new Option<string>("--sort")
            {
                Description = "Sort files by name or language",
                DefaultValueFactory = _ => "name"
            };
            sortOption.AcceptOnlyFromAmong("name", "language");
            sortOption.Aliases.Add("-s");

            //remove - empty - lines
            var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines")
            { Description = "Removing empty lines" };

            removeEmptyLinesOption.Aliases.Add("-r");


            //author
            var authorOption = new Option<string>("--author")
            { Description = "author of the bundle" };
            authorOption.Aliases.Add("-a");



            bundleCommand.Add(languageOption);
            bundleCommand.Add(outputOption);
            bundleCommand.Add(noteOption);
            bundleCommand.Add(sortOption);
            bundleCommand.Add(removeEmptyLinesOption);
            bundleCommand.Add(authorOption);

            var scanner = new FileScanner();
            var bundleService = new BundleService();

            bundleCommand.SetAction(parseResult =>
            {
                var language = parseResult.GetValue(languageOption);

                if (string.IsNullOrWhiteSpace(language))
                {
                    Console.WriteLine("A language must be specified.");
                    return;
                }

                var extensions = language.GetLanguage();

                if (extensions == null)
                {
                    Console.WriteLine($"Unsupported language: {language}");
                    return;
                }

                var sourceDirectory = Directory.GetCurrentDirectory();

                var files = scanner.ScanFiles(sourceDirectory, language!);
                var outputPath = parseResult.GetValue(outputOption);

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    outputPath = Path.Combine(sourceDirectory, "bundle.txt");
                }
                else if (!Path.IsPathRooted(outputPath))
                {
                    outputPath = Path.Combine(sourceDirectory, outputPath);
                }

                var sort = parseResult.GetValue(sortOption) ?? "name";
                var note = parseResult.GetValue(noteOption);
                var removeEmptyLines = parseResult.GetValue(removeEmptyLinesOption);
                var author = parseResult.GetValue(authorOption);

                try
                {
                    
                    bundleService.CreateBundle(
                        files,
                        outputPath,
                        sort,
                        note,
                        removeEmptyLines,
                        author,
                        sourceDirectory);

                    Console.WriteLine($"Bundle created: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating bundle: {ex.Message}");
                }
            }); 

            return bundleCommand;
        }


    }
}
