using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace lab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
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

            SortedDictionary<string, long> sorted = CreateSortedCollection(directory);
            SerializeAndDeserializeCollection(sorted);

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
                    //Path.combine obsługuje znaki separatorów ścieżek
                    DateTime newDate = PrintContentsAndFindOldest(Path.Combine(directoryPath, Path.GetFileName(dir)), tabs); 
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

        static SortedDictionary<string, long> CreateSortedCollection(DirectoryInfo directory)
        {
            var comparer = new FilesComparer();
            var sortedCollection = new SortedDictionary<string, long>(comparer);
            foreach (var file in directory.GetFiles())
            {
                sortedCollection.Add(file.Name, file.Length);
            }

            foreach (var dir in directory.GetDirectories())
            {
                sortedCollection.Add(dir.Name, dir.GetFiles().Length + dir.GetDirectories().Length);
            }
            
            Console.WriteLine("\nSorted collection:");
            foreach (var element in sortedCollection)
            {
                Console.WriteLine($"{element.Key} -> {element.Value}");
            }

            return sortedCollection;
        }

        [Serializable]
        class FilesComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x.Length > y.Length)
                    return 1;
                if (x.Length < y.Length)
                    return -1;
                return string.Compare(x, y, StringComparison.Ordinal);
            }
        }

        static void SerializeAndDeserializeCollection(SortedDictionary<string, long> sorted)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("sorted.bin", FileMode.Create))
            {
                formatter.Serialize(stream, sorted);
            }
            using (FileStream stream = new FileStream("sorted.bin", FileMode.Open))
            {
                SortedDictionary<string, long> newSorted;
                newSorted = (SortedDictionary<string, long>)formatter.Deserialize(stream);
                Console.WriteLine("\nDeserialized collection:");
                foreach (var element in newSorted)
                {
                    Console.WriteLine($"{element.Key} -> {element.Value}");
                }
            }
        }
    }
}