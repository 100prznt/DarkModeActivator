using System.Xml.Linq;

namespace Rca.DarkModeActivator
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var autoClose = false;

            if (args.Length == 0)
                autoClose = args.Any(x => string.Equals(x, "autoclose", StringComparison.OrdinalIgnoreCase));

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
                            var uiThemeElement = xmlFile.Element("FeaturePacks").Element("FeaturePack").Element("Features").Element("Feature");

                            if (!string.Equals(uiThemeElement.Attribute("Default").Value, "True", StringComparison.OrdinalIgnoreCase))
                            {
                                uiThemeElement.Attribute("Default").Value = "True";
                                Console.WriteLine("Darkmode successfully enabled.");
                            }
                            else
                            {
                                Console.WriteLine("Darkmode is already enabled.");
                            }
                            xmlFile.Save(featurepackXmlPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Access to {featurepackXmlPath} failed with exception:");
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            else
                Console.WriteLine($"Local app directory not found, expected path: {rootPath}");

            if (!autoClose)
            {
                Console.WriteLine("Press any key to close.");
                Console.ReadKey();
            }
        }
    }
}