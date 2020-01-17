using System;
using System.IO;
using System.Xml.Serialization;

namespace Toolbox
{
    /// <summary>
    /// Simplified settings manager
    /// </summary>
    public class SettingsManager<T> where T : class
    {
        private readonly string fileName;

        //
        // Create a settings manager
        //
        public SettingsManager(string company, string product)
        {
            fileName = GetFileName(company, product);
        }

        //
        // Save settings
        //
        public void Save(T settings)
        {
            var serializer = new XmlSerializer(settings.GetType());
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, settings);
            }
        }

        //
        // Load settings
        //
        public T Load()
        {
            T settings = null;

            var serializer = new XmlSerializer(typeof(T));
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    settings = serializer.Deserialize(fileStream) as T;
                }
            }
            catch (FileNotFoundException)
            {
            }

            return settings;
        }

        // Build filename
        static private string GetFileName(string company, string product)
        {
            // Base path
            string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Make filename
            string filename = Path.Combine(root, company);
            filename = Path.Combine(filename, product);

            Directory.CreateDirectory(filename);

            filename = Path.Combine(filename, "user.info");

            return filename;
        }
    }
}
