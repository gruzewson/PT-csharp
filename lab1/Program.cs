using System;
using System.IO;

namespace lab1
{
    internal class Program
    {
        public static void PrintContents(string directoryPath, string tabs)
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath);
                string[] directories = Directory.GetDirectories(directoryPath);
                tabs += "  ";
        
                foreach(string file in files)
                {
                    Console.WriteLine(tabs + Path.GetFileName(file));
                }
                foreach (string dir in directories)
                {
                    Console.WriteLine(tabs + Path.GetFileName(dir));
                    PrintContents(directoryPath + "\\" + Path.GetFileName(dir), tabs);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
        }
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                return;
            }

            string directoryPath = args[0];

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Given catalog doesn't exist");
                return;
            }

            Console.WriteLine($"\nCatalog '{directoryPath}' contents:");
            PrintContents(directoryPath, "");
        }
    }
}