namespace LongPathNetCoreApp
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;

    class NetCoreProgram
    {
        static void Main(string[] args)
        {
            var reallyLongDirectory = @"C:\temp\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var version = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            Console.WriteLine($"Target Framework: '{version}'");

            Console.WriteLine($"Environment Version: {Environment.Version}");
            Console.WriteLine($"Framework Description: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");

            Console.WriteLine($"{Environment.NewLine}");

            // Find of create some files and directories with long names
            if (Directory.Exists(reallyLongDirectory))
            {
                Console.WriteLine($"The Really-Long-Directory exists:{Environment.NewLine}{reallyLongDirectory}");
            }
            else
            {
                Console.WriteLine($"Creating a directory that is {reallyLongDirectory.Length} characters long{Environment.NewLine}{reallyLongDirectory}");
                var dirInfo = Directory.CreateDirectory(reallyLongDirectory);

                var dirs = Enumerable.Range(1, 3).Select(i => $"New Folder ({i})").ToList();
                Console.WriteLine($"Creating some directories: {string.Join(", ", dirs)}");
                dirs.ForEach(dir => Directory.CreateDirectory(reallyLongDirectory + "\\" + dir));

                var files = Enumerable.Range(1, 3).Select(i => $"New File ({i}).txt").ToList();
                Console.WriteLine($"Creating some files: {string.Join(", ", files)}");
                files.ForEach(f => File.Create(reallyLongDirectory + "\\" + f));
            }

            // List directories if any
            Console.WriteLine();
            var dirsGet = Directory.GetDirectories(reallyLongDirectory);
            if (dirsGet != null && dirsGet.Length == 0)
            {
                Console.WriteLine("There is no directories found.");
            }
            else
            {
                var dirInfos = dirsGet.Select(f => new DirectoryInfo(f));
                Console.WriteLine($"{Environment.NewLine}*** Directories list: {Environment.NewLine}" +
                    $"{string.Join(Environment.NewLine, dirInfos.Select(di => $"'{di.Name}'"))}");
            }

            // List files if any
            Console.WriteLine();
            var filesGet = Directory.GetFiles(reallyLongDirectory);
            if (filesGet.Length == 0)
            {
                Console.WriteLine("There is no files found.");
            }
            else
            {
                var fileInfos = filesGet.Select(f => new FileInfo(f));
                var id = 1;
                var fiDetalis = fileInfos.Select(fi => $"{id++}\t{Math.Round(ConvertBytesToMegabytes(fi.Length), 2)} MB\t'{fi.FullName}'");

                Console.WriteLine($"{Environment.NewLine}*** Files list: {Environment.NewLine}" +
                    $"{string.Join(Environment.NewLine, fiDetalis)}");
            }

            Console.ReadKey();
        }

        private static double ConvertBytesToMegabytes(long bytes) => (bytes / 1024f) / 1024f;
    }
}
