using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace practicod1_pnini_rosental.Commands
{


    public class CreateRspCommand
    {

        private static bool AskYesNo(string question)
        {
            while (true)
            {
                Console.Write($"{question} (yes/no): ");
                string? input = Console.ReadLine();

                if (string.Equals(input, "yes", StringComparison.OrdinalIgnoreCase))
                    return true;

                if (string.Equals(input, "no", StringComparison.OrdinalIgnoreCase))
                    return false;

                Console.WriteLine("Please enter yes or no.");
            }
        }

        public static Command Create()
        {
            string[] languages =
{
    "C#", "Python", "Java", "JavaScript",
    "TypeScript", "C", "C++", "All"
};

            string[] sortingOptions =
            {
    "name", "language"
};
            var rspCommand = new Command("create-rsp", "creating a response file");
            rspCommand.Aliases.Add("-c");
            rspCommand.SetAction(ll =>
            {




                Console.WriteLine("\n=== Create Response File ===");
                Console.WriteLine("Choose a programming language:");
                
                for (int i = 0; i < languages.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {languages[i]}");
                }

                
                int languageIndex;

                while (true)
                {
                    Console.Write("Your choice: ");
                    string? languageChoice = Console.ReadLine();

                    if (int.TryParse(languageChoice, out languageIndex) &&
                        languageIndex >= 1 &&
                        languageIndex <= languages.Length)
                    {
                        break;
                    }

                    Console.WriteLine("Invalid choice. Please try again.");
                }

                string language = languages[languageIndex - 1];

                Console.WriteLine("\nChoose sorting method:");

                for (int i = 0; i < sortingOptions.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {sortingOptions[i]}");
                }

                
                string sorting;

                while (true)
                {
                    Console.Write("Your choice (default: 1): ");
                    string? sortingChoice = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(sortingChoice))
                    {
                        sorting = "name";
                        break;
                    }

                    if (int.TryParse(sortingChoice, out int sortingIndex) &&
                        sortingIndex >= 1 &&
                        sortingIndex <= sortingOptions.Length)
                    {
                        sorting = sortingOptions[sortingIndex - 1];
                        break;
                    }

                    Console.WriteLine("Invalid choice. Please try again.");
                }

                Console.Write("\nOutput file path (default: bundle.txt): ");
                string output = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(output))
                {
                    output = "bundle.txt";
                }
           
                bool note = AskYesNo("Add notes?");
             
                bool removeEmptyLines = AskYesNo("Remove empty lines?");
                Console.Write("Author name (optional): ");
                string? author = Console.ReadLine();

                Console.Write("\nResponse file name (default: bundle.rsp): ");
                string? rspFile = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(rspFile))
                {
                    rspFile = "bundle.rsp";
                }

                var arguments = new List<string>
{
                        "bundle",
    "--language",
    language,
    "--output",
    $"\"{output}\"",
    "--sort",
    sorting
};
                if (note)
                {
                    arguments.Add("--note");
                }

                if (removeEmptyLines)
                {
                    arguments.Add("--remove-empty-lines");
                }

                if (!string.IsNullOrWhiteSpace(author))
                {
                    arguments.Add("--author");
                    arguments.Add($"\"{author}\"");
                }

                try
                {
                    File.WriteAllLines(rspFile, arguments);
                    Console.WriteLine($"Response file created: {rspFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating response file: {ex.Message}");
                }
            });
            return rspCommand;
        }


    }
}
