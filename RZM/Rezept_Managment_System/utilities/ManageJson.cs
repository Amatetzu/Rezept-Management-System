using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;



namespace Rezept_Managment_System.utilities
{
    internal class ManageJson
    {
        public const string ZutatenPath = "Data/Zutaten.json";
        public const string RezeptePath = "Data/Rezepte.json";
        public const string KategorienPath = "Data/Kategorien.json";
        public static List<string> SplitString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            // Text anhand von Semikolon, Komma aufteilen (Regex)
            string[] parts = Regex.Split(input, @"[;,]+");
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
                MessageBox.Show($"Fehler beim Lesen der Datei: {ex.Message}");
                return default;
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
                MessageBox.Show($"Fehler beim Schreiben der Datei: {ex.Message}");
            }
        }



        public static List<string> AllSearch(string relativePath, string propertyName) // gibt eine Liste von Strings zurück 
        {
            List<string> resultList = new List<string>();

            using (StreamReader file = File.OpenText(relativePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JArray jsonArray = (JArray)JToken.ReadFrom(reader);

                foreach (JObject item in jsonArray)
                {
                    // Hier propertyName verwenden, um den Wert zu extrahieren
                    string value = item.GetValue(propertyName)?.ToString();
                    if (value != null)
                    {
                        resultList.Add(value);
                    }
                }
            }

            return resultList;
        }

    }

}