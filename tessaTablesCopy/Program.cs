using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace tessaTablesCopy
{
    class Program
    {
        static void Main()
        {
            string path;
            string fileName;
            string newFileName;
            string text;
            string pathFileCopied;
            string fileExtension = ".tst";
            List<string> textCopiedArray = new List<string>();

            Console.WriteLine("Specify the path:");
            path = Console.ReadLine();

            Console.WriteLine("Specify the file name:");
            fileName = Console.ReadLine();

            Console.WriteLine("Specify the new file name:");
            newFileName = Console.ReadLine();

            if (string.IsNullOrEmpty(newFileName))
            {
                newFileName = fileName + "Copy";
            }

            try
            {
                path = Path.GetFullPath(path);
                string pathCombine = Path.Combine(path, fileName);

                if (!File.Exists(pathCombine))
                {
                    pathCombine = Path.Combine(path, fileName + fileExtension);
                }

                using StreamReader reader = new StreamReader(pathCombine);
                text = reader.ReadToEnd();
                string output;
                string[] words = text.Split(' ');
                text = text.Replace(fileName, newFileName);
                words = text.Split(' ');

                List<string> duplicatesIds = new List<string>();

                foreach (var word in words)
                {
                    if (word.Contains("ID=") && word.Length <= 42)
                    {
                        var duplicatesId = word.Replace("ID=", string.Empty).Replace("\"", string.Empty);

                        if (!duplicatesIds.Any(x => x == duplicatesId) && words.Count(x => x.Contains(duplicatesId)) > 1)
                        {
                            string tempTableID = Guid.NewGuid().ToString();
                            text = text.Replace(duplicatesId, tempTableID);
                            duplicatesIds.Add(tempTableID);
                            duplicatesIds.Add(duplicatesId);
                        }
                    }
                }

                words = text.Split(' ');

                foreach (var word in words)
                {
                    if (!duplicatesIds.Any(x => word.Contains(x)) && word.Contains("ID=") && word.Length <= 42)
                    {
                        output = $"ID=\"{Guid.NewGuid()}\"";
                    }
                    else
                    {
                        output = word;
                    }

                    textCopiedArray.Add(output);
                }

                newFileName += fileExtension;
                int doteIndex = newFileName.LastIndexOf(".");

                if (doteIndex > 0)
                {
                    fileExtension = newFileName.Substring(newFileName.LastIndexOf("."));
                    newFileName = newFileName.Substring(0, newFileName.LastIndexOf("."));
                    pathFileCopied = Path.Combine(path, $"{newFileName}{fileExtension}");
                }
                else
                {
                    pathFileCopied = Path.Combine(path, $"{newFileName}");
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
