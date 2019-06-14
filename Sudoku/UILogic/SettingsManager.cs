using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Sudoku.UILogic
{
    public class SettingsManager
    {
        private readonly string fileName;

        public SettingsManager(string company, string product)
        {
            fileName = GetFileName(company, product);
        }

        public void Save(object settings)
        {
            var serializer = new XmlSerializer(settings.GetType());
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, settings);
            }
        }

        public T Load<T>() where T : class
        {
            T settings = default(T);

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

        private string GetFileName(string company, string product)
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
