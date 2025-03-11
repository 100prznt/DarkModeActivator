using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace Rca.DarkModeActivator
{
    static class Program
    {
        /// <summary>
        /// Patch theme-featurepacks.xml to enable the dark mode in Autodesk Fusion.
        /// </summary>
        /// <param name="args">AutoClose: Close the commandline window after execution.</param>
        public static void Main(string[] args)
        {
            #region Startup
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            var attribute = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).Cast<AssemblyDescriptionAttribute>().FirstOrDefault();
            var appName = $"{typeof(Program).Assembly.GetName().Name} v{versionInfo.ProductVersion}";

            Console.Title = "DMA";

            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(appName);
            Console.ResetColor();
            if (attribute is not null)
                Console.WriteLine(attribute.Description);
            Console.WriteLine();
            Console.WriteLine(versionInfo.LegalCopyright);
            Console.WriteLine(String.Empty.PadLeft(72, '-'));
            Console.WriteLine();
            #endregion
            
            var autoClose = false;

            if (args.Length == 0)
                autoClose = args.Any(x => string.Equals(x, "AutoClose", StringComparison.OrdinalIgnoreCase));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var rootPath = Path.Combine(path, "Autodesk", "webdeploy", "production");

            if (Directory.Exists(rootPath))
            {
                foreach (var deployPath in Directory.GetDirectories(rootPath))
                {
                    var featurepackXmlPath = Path.Combine(deployPath, "Neutron", "UI", "Base", "Resources", "UIToolkit", "theme-featurepacks.xml");
                    if (File.Exists(featurepackXmlPath))
                    {
                        try
                        {
                            var xmlFile = XDocument.Load(featurepackXmlPath);
                            var uiThemeElement = xmlFile?.Element("FeaturePacks")?.Element("FeaturePack")?.Element("Features")?.Element("Feature");

                            if (uiThemeElement is not null && !string.Equals(uiThemeElement.Attribute("Default")?.Value, "True", StringComparison.OrdinalIgnoreCase))
                            {
                                uiThemeElement.Attribute("Default").Value = "True";
                                WriteLine("theme-featurepacks.xml successfully patched, Darkmode is enabled.", ConsoleColor.Green);
                            }
                            else
                                WriteLine("theme-featurepacks.xml is already patched, Darkmode is enabled.", ConsoleColor.Cyan);

                            xmlFile.Save(featurepackXmlPath);
                        }
                        catch (Exception ex)
                        {
                            WriteLine($"Access to {featurepackXmlPath} failed with exception:", ConsoleColor.Red);
                            WriteLine(ex.ToString());
                        }
                    }
                }
            }
            else
                WriteLine($"Local app directory not found, expected path: {rootPath}", ConsoleColor.Red);

            if (!autoClose)
            {
                WriteLine();
                WriteLine(string.Empty.PadLeft(72, '-'));
                Console.Write("Press any key to close...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        private static void WriteLine() => Console.WriteLine();

        /// <summary>
        /// Writes the specified string value to the console with the specified color.
        /// </summary>
        /// <param name="message">Message to print out</param>
        /// <param name="color">Foreground color</param>
        private static void WriteLine(string message, ConsoleColor? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;

            Console.WriteLine(message);

            if (color.HasValue)
                Console.ResetColor();
        }
    }
}
