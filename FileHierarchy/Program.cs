using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    class Directories
    {
        public string Name { get; set; }
        public string DataCreated { get; set; }
        public List<Files> Files { get; set; }
    }
    class Files
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }

    }
    class FileHierarchy
    {
        public List<string> GetAllFiles(string sDirt)
        {
            List<string> directory = new List<string>();

            InformationDirection(sDirt);

            try
            {
                foreach (string fl in Directory.GetDirectories(sDirt))
                {
                    directory.AddRange(GetAllFiles(fl));
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return directory;

        }

        public void InformationDirection(string direc)
        {
            DirectoryInfo directory = new DirectoryInfo(direc);

            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter writetext = new StreamWriter(@"D:\Test.json", true))
            {

                using (JsonWriter writer = new JsonTextWriter(writetext))
                {
                    if (directory.Exists)
                    {
                        Directories directories = new Directories { Name = directory.Name, DataCreated = directory.CreationTime.ToString(), Files = GetFiles(direc) };
                        writer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, directories);

                    }
                    else
                    {
                        writetext.WriteLine($"{directory.FullName}  does not exist");

                        Console.WriteLine($"{directory.FullName}  does not exist");
                    }
                    serializer.Serialize(writer, "Children :");
                }

            }

        }

        public List<Files> GetFiles(string direc)
        {
            DirectoryInfo directory = new DirectoryInfo(direc);
            List<Files> list = new List<Files>();
            FileInfo[] files = directory.GetFiles("*.*");

            foreach (FileInfo file in files)
            {
                list.Add(new Files { Name = file.Name, Size = (int)file.Length, Path = file.DirectoryName });

            }

            return list;

        }



    }

    class Program
    {
        static void Main()
        {
            FileHierarchy fileHierarchy = new FileHierarchy();
            if (File.Exists(@"D:\Test.json"))
            {
                File.Delete(@"D:\Test.json");
                fileHierarchy.GetAllFiles(@"D:\Test");
            }
            else
                fileHierarchy.GetAllFiles(@"D:\Test");

            Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}