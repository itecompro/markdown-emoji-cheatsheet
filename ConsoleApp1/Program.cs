﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly string filesDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Files");

        // This file is manually created from https://api.github.com/emojis whenever we want to update the list
        // Last update was from v8 of the api
        private static readonly IEnumerable<string> codes = File.ReadAllLines($"{filesDirectory}/Codes.txt");

        // Categories
        private static readonly string categoriesDirectory = Path.Combine(filesDirectory, "Categories");
        private static readonly IEnumerable<string> activities = File.ReadAllLines($"{categoriesDirectory}/Activities.txt");
        private static readonly IEnumerable<string> flags = File.ReadAllLines($"{categoriesDirectory}/Flags.txt");
        private static readonly IEnumerable<string> food = File.ReadAllLines($"{categoriesDirectory}/Food.txt");
        private static readonly IEnumerable<string> nature = File.ReadAllLines($"{categoriesDirectory}/Nature.txt");
        private static readonly IEnumerable<string> objects = File.ReadAllLines($"{categoriesDirectory}/Objects.txt");
        private static readonly IEnumerable<string> people = File.ReadAllLines($"{categoriesDirectory}/People.txt");
        private static readonly IEnumerable<string> places = File.ReadAllLines($"{categoriesDirectory}/Places.txt");
        private static readonly IEnumerable<string> symbols = File.ReadAllLines($"{categoriesDirectory}/Symbols.txt");

        static void Main(string[] args)
        {
            GenerateOutputForWiki();
        }

        static void GenerateUncategorizedCodes()
        {
            var uncategorizedCodes = new List<string>();

            foreach (var code in codes)
                if (!activities.Union(flags).Union(food).Union(nature).Union(objects).Union(people).Union(places).Union(symbols).Contains(code))
                    uncategorizedCodes.Add(code);

            File.WriteAllLines($"{filesDirectory}/UncategorizedCodes.txt", uncategorizedCodes);
        }

        static void GenerateOutputForWiki()
        {
            var result = new List<string>
            {
                "If you see `unordered` somewhere, any items after that are not in the right place and need to be replaced. Please see [How to change](https://github.com/itecompro/markdown-emoji-cheatsheet#how-to-change).",
                "",
                "- [Activities](#Activities)",
                "- [Flags](#Flags)",
                "- [Food](#Food)",
                "- [Nature](#Nature)",
                "- [Objects](#Objects)",
                "- [People](#People)",
                "- [Places](#Places)",
                "- [Symbols](#Symbols)",
                ""
            };

            result.Add("# Activities");
            result.Add("");
            result.AddRange(GetTable(activities));
            result.Add("");

            result.Add("# Flags");
            result.Add("");
            result.AddRange(GetTable(flags));
            result.Add("");

            result.Add("# Food");
            result.Add("");
            result.AddRange(GetTable(food));
            result.Add("");

            result.Add("# Nature");
            result.Add("");
            result.AddRange(GetTable(nature));
            result.Add("");

            result.Add("# Objects");
            result.Add("");
            result.AddRange(GetTable(objects));
            result.Add("");

            result.Add("# People");
            result.Add("");
            result.AddRange(GetTable(people));
            result.Add("");

            result.Add("# Places");
            result.Add("");
            result.AddRange(GetTable(places));
            result.Add("");

            result.Add("# Symbols");
            result.Add("");
            result.AddRange(GetTable(symbols));
            result.Add("");

            File.WriteAllLines($"{filesDirectory}/Home.md", result);
        }

        static IEnumerable<string> GetTable(IEnumerable<string> codes)
        {
            var result = new List<string>();
            result.Add("|   |   |   |");
            result.Add("|---|---|---|");

            var counter = 1;
            var resultLine = new StringBuilder();
            foreach (var code in codes)
            {
                resultLine.Append($"| {GetCellContent(code)} ");
                if (counter == 3)
                {
                    resultLine.Append("|");
                    result.Add(resultLine.ToString());
                    resultLine = new StringBuilder();
                    counter = 1;
                }
                else
                    counter++;
            }

            if (counter != 1)
                result.Add(resultLine.ToString());

            return result;
        }

        static string GetCellContent(string code)
        {
            return $":{code}: `:{code}:`";
        }
    }
}
