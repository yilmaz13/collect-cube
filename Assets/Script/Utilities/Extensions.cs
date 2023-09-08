using FullSerializer;
using System.IO;

namespace Utilities
{
    public class Extensions
    {
        public static T LoadJsonFile<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var file = new StreamReader(path);
            var fileContents = file.ReadToEnd();
            var data = fsJsonParser.Parse(fileContents);
            object deserialized = null;
            var serializer = new fsSerializer();
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            file.Close();
            return deserialized as T;
        }
    }
}