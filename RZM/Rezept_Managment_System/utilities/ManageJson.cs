using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rezept_Managment_System.utilities
{
    internal class ManageJson
    {
        public static List<string> SplitString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            // Text anhand von Semikolon, Komma aufteilen (Regex)
            string[] parts = System.Text.RegularExpressions.Regex.Split(input, @"[;,]+");
            return new List<string>(parts);

        }

        public static T ReadJsonFile<T>(string fullPath)
        {
            try
            {
                if (!System.IO.File.Exists(fullPath))
                {
                    throw new System.IO.FileNotFoundException("Die Datei wurde nicht gefunden.", fullPath);
                }

                string jsonContent = System.IO.File.ReadAllText(fullPath);
                T deserializedObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonContent);
                return deserializedObject;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fehler beim Lesen der Datei: {ex.Message}");
                return default(T);
            }
        }
        
        public static void WriteJsonFile<T>(string fullPath, T objectToWrite)
        {
            try
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(objectToWrite, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(fullPath, jsonContent);
                MessageBox.Show("Zutat wurde gespeichert");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fehler beim Schreiben der Datei: {ex.Message}");
            }
        }

        public static string GetfullPath(string relativPath)
        {
            string fullPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, relativPath);
            return fullPath;

        }
    }
}
