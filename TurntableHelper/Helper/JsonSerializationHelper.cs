using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TurntableHelper.Helper
{
    public static class JsonSerializationHelper
    {
        public static T DeserializeFromFile<T>(string filePath) where T : class
        {
            if(File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    T result = JsonConvert.DeserializeObject<T>(jsonContent);
                    return result;
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                
            }
            return null;
        }

        public static void SerializeToFile<T>(T obj, string filePath) where T : class
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string jsonContext = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(filePath, jsonContext);
            }catch (Exception ex) { Console.WriteLine(ex.ToString());}
        }
    }
}
