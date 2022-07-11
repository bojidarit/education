namespace CommonLib
{
    using System.IO;
    using System.Reflection;

    public static class Common
    {
        public static string GetHtmlFromResource(Assembly assembly, string resourceName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public static string GetHtmlFromResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"CommonLib.{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
