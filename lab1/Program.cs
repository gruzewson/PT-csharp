using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace lab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                return;
            }

            string directoryPath = args[0];
            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Given catalog doesn't exist");
                return;
            }

            Console.WriteLine($"\nCatalog '{directoryPath}' contents:");
            DateTime oldestDate = PrintContentsAndFindOldest(directoryPath, "");
            Console.WriteLine($"\nOldest file: {oldestDate}\n");
        }
        
        static DateTime PrintContentsAndFindOldest(string directoryPath, string tabs)
        {
            DateTime oldestDate = DateTime.MaxValue;
            try
            {
                string[] files = Directory.GetFiles(directoryPath);
                string[] directories = Directory.GetDirectories(directoryPath);
                tabs += "  ";

                foreach (string file in files)
                {
                    FileSystemInfo info = new FileInfo(file);
                    FileInfo fi = new FileInfo(file);
                    Console.WriteLine($"{tabs} {Path.GetFileName(file)} {fi.Length} bytes {GetAttributes(info)}");
                    DateTime fileCreatedDate = File.GetCreationTime(file);
                    oldestDate = oldestDate < fileCreatedDate ? oldestDate : fileCreatedDate;
                }
                foreach (string dir in directories)
                {
                    int size = Directory.GetDirectories(dir).Length + Directory.GetFiles(dir).Length;
                    Console.WriteLine($"{tabs} {Path.GetFileName(dir)} ({size}) ----");
                    DateTime newDate = PrintContentsAndFindOldest(Path.Combine(directoryPath, Path.GetFileName(dir)), tabs); //Path.combine obsługuje znaki separatorów ścieżek
                    oldestDate = oldestDate < newDate ? oldestDate : newDate;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            return oldestDate;
        }
        static string GetAttributes(FileSystemInfo info)
        {
            string attributes = "";
            if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                attributes += "r";
            else
                attributes += "-";
            if ((info.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
                attributes += "a";
            else
                attributes += "-";
            if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                attributes += "h";
            else
                attributes += "-";
            if ((info.Attributes & FileAttributes.System) == FileAttributes.System)
                attributes += "s";
            else
                attributes += "-";
            return attributes;
        } 
        
    }
}