using System.IO;
using Newtonsoft.Json;

namespace Library.Utilities
{
    public class Persistence
    {
        /// <summary>
        /// Creates a file if it does not exist, will also create the directory
        /// </summary>
        /// <param name="path">path to the file, including the file itself</param>
        public static void CreateIfNotExists(string path)
        {
            //Check if directory exists, if not, we should make it
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                { Directory.CreateDirectory(Path.GetDirectoryName(path)); }

            //Now check if the file exists and create it if not
            if (!File.Exists(path))
                { File.Create(path).Dispose(); }
        }

        /// <summary>
        /// Overwrites a json file with new content
        /// </summary>
        /// <param name="obj">The object to serialize to json</param>
        /// <param name="path">path to the json file to export to</param>
        public static void OverwriteJson(object obj, string path)
        {
            //Now we can write to the file
            using (StreamWriter sw = File.CreateText(path))
                { sw.WriteLine(JsonConvert.SerializeObject(obj)); }
        }

        /// <summary>
        /// Reads information from a json file
        /// NOTE: If the json is unreadable, we return a new object of type T
        /// </summary>
        /// <param name="path">Path to the json file</param>
        /// <typeparam name="T">Type of data structure we should return from the json</typeparam>
        /// <returns>Object of type T from the json</returns>
        public static T LoadFromJson<T>(string path) where T : new()
        {
            //If the file doesn't exist we just return a new object
            if (!File.Exists(path))
                { return new T(); }

            //Otherwise we load the file and return the data
            string json = null;

            using (StreamReader sr = File.OpenText(path))
                { json = sr.ReadToEnd(); }

            return JsonConvert.DeserializeObject<T>(json) ?? new T();
        }
    }
}
