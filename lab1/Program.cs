using System;
using System.IO;

namespace lab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguements");
                return;
            }

            string directoryPath = args[0];

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Given catalog doesn't exist");
                return;
            }

            Console.WriteLine($"Catalog contents: {directoryPath}");
            
            string[] files = Directory.GetFiles(directoryPath);
            string[] directories = Directory.GetDirectories(directoryPath);

            Console.WriteLine("Files:");
            foreach (string file in files)
            {
                Console.WriteLine("\t" + Path.GetFileName(file));
            }

            Console.WriteLine("Subcatalogs:");
            foreach (string dir in directories)
            {
                Console.WriteLine("\t" + Path.GetFileName(dir));
            }
        }
    }
}