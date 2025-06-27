using System.Diagnostics;

namespace ImageToConsole
{
#pragma warning disable CA1416 // Validate platform compatibility
    internal class Program
    {
        static void Main(string[] args)
        {
            string saveDir = "c:\\tmp\\";
            Console.WriteLine("Get GPS coordinates from images");
            Console.WriteLine($"Output CSV will be saved to {saveDir}");
            if (Directory.Exists(saveDir) == false)
            {
                Console.WriteLine("The output folder does not exist. Please create it.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            string? filePath = null;
            string? dirPath = null;
            string inputpath = "";

            List<string> files = [];

            if (args.Length == 0)
            {
                Console.Write("Image or directory path: ");
                inputpath = Console.ReadLine()+"";
            }
            else
            {
                inputpath = args[0];
            }

            inputpath = inputpath.Replace("\"", "");

            if (File.Exists(inputpath))
            {
                filePath = inputpath;
            }
            else if (Directory.Exists(inputpath))
            {
                dirPath = inputpath;
            }
            else
            {
                Console.WriteLine("Invalid path, exiting");
                return;
            }

            if (filePath != null)
            {
                files.Add(filePath);
            }
            else if (dirPath != null)
            {
                foreach (string file in Directory.GetFiles(dirPath))
                {
                    //Debug.WriteLine($"Adding file: {file}");
                    files.Add(file);
                }
            }

            List<GPSData> gpsList = [];

            Console.WriteLine($"Scanning {files.Count} files, please wait.");
            int consoleY = Console.CursorTop;
            int count = 1;
            foreach (string file in files)
            {
                Console.SetCursorPosition(0, consoleY);
                Console.Write($"{count}/{files.Count} scanned");
                //Debug.WriteLine($"Print GPS, file: {file}");
                if (IsImage(file))
                {
                    //Console.WriteLine($"------ {file}");
                    //ImageParser.PrintGPSData(file);
                    GPSData? gps = ImageParser.GetGPSData(file);
                    if (gps == null)
                    {
                        gps = new GPSData(file);
                    }
                    gpsList.Add(gps);
                }
                count++;
            }
            Console.WriteLine();

            if (gpsList.Count > 0)
            {
                string savePath = @$"{saveDir}gps_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}_{DateTime.Now.Millisecond}.csv";
                Console.WriteLine($"Saving csv to: {savePath}");

                File.WriteAllText(savePath, ImageParser.GPSDataToCsv(gpsList));
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            //ConsoleKeyInfo key = Console.ReadKey();
            //if (key.Key == ConsoleKey.V)
            //{
            //    ImageParser parser = new(filePath);
            //    parser.WriteImageToConsole(0, Console.CursorTop, Console.BufferWidth, Console.BufferHeight - 3 - Console.CursorTop, true);
            //}
        }

        private static bool IsImage(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            if (extension == ".jpg") return true;
            if (extension == ".jpeg") return true;
            if (extension == ".png") return true;
            if (extension == ".webp") return true;
            return false;
        }

    }


#pragma warning restore CA1416 // Validate platform compatibility
}
