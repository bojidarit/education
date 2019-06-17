namespace AppRuntimeVer
{
	using System.Linq;

	class Program
	{
		static void Main(string[] args)
		{
			System.Reflection.Assembly assembly = null;

			if (args != null && args.Length > 0)
			{
				assembly = System.Reflection.Assembly.LoadFrom(args[0]);

				System.Console.WriteLine("{0} - Assembly Image Runtime Version",
					assembly.ImageRuntimeVersion);
				System.Console.WriteLine("{0} - Environment Version", System.Environment.Version);
			}
			else
			{
				assembly = System.Reflection.Assembly.GetExecutingAssembly();

				System.Console.WriteLine("Hint: Put assembly name as application argument");
				System.Console.WriteLine("{0} - Current Assembly Image Runtime Version", assembly.ImageRuntimeVersion);
			}

			System.Console.WriteLine("----------------------------------------");
			System.Console.WriteLine("Help: Identifies the version of the .NET Framework that a particular assembly was compiled against.");
			object[] list = assembly.GetCustomAttributes(true);
			var attribute = list.OfType<System.Runtime.Versioning.TargetFrameworkAttribute>().FirstOrDefault();

			if (attribute != null)
			{
				System.Console.WriteLine(attribute.FrameworkName);
				System.Console.WriteLine(attribute.FrameworkDisplayName);
			}

			System.Console.ReadKey();
		}
	}
}
