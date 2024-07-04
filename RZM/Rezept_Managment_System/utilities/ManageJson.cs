using System;
using System.Collections.Generic;
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

       

        public static Zutat SearchIngredient(string relativePath, string name)
        {
            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath); ;
            List<Zutat> zutaten = ReadJsonFile<List<Zutat>>(fullPath);

            if (zutaten == null)
            {
                MessageBox.Show("Die Zutatenliste konnte nicht geladen werden.");
                return null;
            }

            Zutat result = zutaten.FirstOrDefault(z => z.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (result == null)
            {
                MessageBox.Show($"Die Zutat '{name}' wurde nicht gefunden.");
            }

            return result;
        }
    }

}