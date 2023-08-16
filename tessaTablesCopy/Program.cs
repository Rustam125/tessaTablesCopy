using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tessaTablesCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            string fileName;
            string text;
            string pathFileCopied;
            string fileExtension = ".tst";
            List<string> textCopiedArray = new List<string>();

            Console.WriteLine("Specify the path:");
            path = Console.ReadLine();
            Console.WriteLine("Specify the file name:");
            fileName = Console.ReadLine();

            try
            {
                path = Path.GetFullPath(path);
                string pathCombine = Path.Combine(path, fileName);

                if (!File.Exists(pathCombine))
                {
                    fileName += fileExtension;
                    pathCombine = Path.Combine(path, fileName);
                }

                using StreamReader reader = new StreamReader(pathCombine);
                text = reader.ReadToEnd();
                string output;

                foreach (var word in text.Split(' '))
                {
                    if (word.Contains("ID=") && word.Length <= 42)
                    {
                        output = $"ID=\"{Guid.NewGuid()}\"";
                    }
                    else
                    {
                        output = word;
                    }

                    textCopiedArray.Add(output);
                }

                int doteIndex = fileName.LastIndexOf(".");

                if (doteIndex > 0)
                {
                    fileExtension = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                    pathFileCopied = Path.Combine(path, $"{fileName}Copy{fileExtension}");
                }
                else
                {
                    pathFileCopied = Path.Combine(path, $"{fileName}Copy");
                }


                using FileStream fs = File.Create(pathFileCopied);
                byte[] info = new UTF8Encoding(true).GetBytes(string.Join(' ', textCopiedArray));
                fs.Write(info, 0, info.Length);

                Console.WriteLine("File has been copied successfully!");
                Console.WriteLine($"Path: {pathFileCopied}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
