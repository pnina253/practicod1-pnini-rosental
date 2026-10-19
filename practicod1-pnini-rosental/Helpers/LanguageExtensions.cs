using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicod1_pnini_rosental.Helpers
{
    public static class LanguageExtensions
    {
        public static string[]? GetLanguage(this string language)
        {
            switch (language.ToUpperInvariant())
            {
                case "CSHARP":
                case "C#":
                    return [".cs"];

                case "PYTHON":
                    return [".py"];

                case "JAVA":
                    return [".java"];

                case "JAVASCRIPT":
                    return [".js"];

                case "TYPESCRIPT":
                    return [".ts"];

                case "C":
                    return [".c"];
                case "C++":
                case "CPP":
                    return [".cpp"];

                case "ALL":
                    return [".cs", ".py", ".java", ".js", ".ts", ".c", ".cpp"];

                default:
                    return null;
            }
        }
    }
}
