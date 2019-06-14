using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Sudoku.UILogic
{
    static public class SettingsManager
    {
        static private string fileName;

        static public void Save(object settings)
        {
            if (fileName == null)
            {
                fileName = GetFileName();
            }

            var serializer = new XmlSerializer(settings.GetType());
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, settings);
            }
        }

        static public object Load(object settings)
        {
            if (fileName == null)
            {
                fileName = GetFileName();
            }

            var serializer = new XmlSerializer(settings.GetType());
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    settings = serializer.Deserialize(fileStream);
                }
            }
            catch (FileNotFoundException)
            {
                settings = null;
            }

            return settings;
        }

        static private string GetFileName()
        {
            // Base path
            string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Find assembly
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            // Find company name
            string companyName = "NoCompany";
            AssemblyCompanyAttribute[] attrs = (AssemblyCompanyAttribute[])assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);

            if ((attrs != null) && attrs.Length > 0 && attrs[0].Company.Length > 0)
            {
                companyName = attrs[0].Company;
            }

            // Find product name
            string productName = AppDomain.CurrentDomain.FriendlyName;

            // Make filename
            string filename = Path.Combine(root, companyName);
            filename = Path.Combine(filename, productName);

            Directory.CreateDirectory(filename);

            filename = Path.Combine(filename, "user.info");

            return filename;
        }
    }
}
