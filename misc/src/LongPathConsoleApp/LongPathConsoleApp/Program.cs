namespace LongPathConsoleApp
{
    using System;
    using System.IO;

    /// <summary>
    /// Source: https://docs.microsoft.com/en-us/archive/blogs/jeremykuhne/net-4-6-2-and-long-paths-on-windows-10
    /// Note: Did not work for me on the work computer...
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var reallyLongDirectory = @"C:\temp\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (Directory.Exists(reallyLongDirectory))
            {
                Console.WriteLine($"The Really-Long-Directory exists:{Environment.NewLine}{reallyLongDirectory}");
            }

            var dirs = Directory.GetDirectories(reallyLongDirectory);

            var files = Directory.GetFiles(reallyLongDirectory);
            if (files != null && files.Length > 0)
            {
                Console.WriteLine($"Really-Long-Directory{Environment.NewLine}{reallyLongDirectory}" +
                    $"{Environment.NewLine}files list{Environment.NewLine}" +
                    $"{string.Join(Environment.NewLine, files)}");
            }

            //Console.WriteLine($"Creating a directory that is {reallyLongDirectory.Length} characters long");
            //Directory.CreateDirectory(reallyLongDirectory);

            Console.ReadKey();
        }
    }
}
