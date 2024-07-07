using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RZM_MVVM_.Modell
{
    public static class JsonUtils
    {

        public static void WriteJson(string filePath, object dataToAppend)
        {
            // Lesen der vorhandenen Daten aus der Datei
            string json;
            if (File.Exists(filePath))
            {
                json = File.ReadAllText(filePath, Encoding.UTF8);
            }
            else
            {
                // Wenn die Datei nicht existiert, erstelle eine leere JSON-Datei
                json = "[]";
            }

            // Deserialisieren der vorhandenen JSON-Daten in eine Liste oder Array
            var existingData = JsonConvert.DeserializeObject<List<object>>(json);

            // Hinzufügen der neuen Daten
            existingData.Add(dataToAppend);

            // Serialisieren der kombinierten Daten zurück in JSON
            var updatedJson = JsonConvert.SerializeObject(existingData, Formatting.Indented);

            // Schreiben der aktualisierten Daten zurück in die Datei
            File.WriteAllText(filePath, updatedJson, Encoding.UTF8);
        }

        public static List<string> ExtractStringListFromJson(string path, string attributeName = "name") // extrahiert eine liste von strings aus einer json datei per default ist es name
        {
            List<string> resultList = new List<string>();

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Die JSON-Datei wurde nicht gefunden.", path);
                }

                using (StreamReader file = File.OpenText(path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JArray jsonArray = JArray.Load(reader);

                    foreach (JObject item in jsonArray.Children<JObject>())
                    {
                        if (item.TryGetValue(attributeName, out JToken value))
                        {
                            resultList.Add(value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Fehler beim Lesen der JSON-Datei: {ex.Message}");
                MessageBox.Show($"Fehler beim Lesen der JSON-Datei: {ex.Message}");
            }

            return resultList;
        }

        public static List<string> FilterList(List<string> list, string filter) // sucht aus der liste alle namen die den filter enthalten
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var lowerFilter = filter.ToLower();
            var filteredList = from item in list
                               where item.ToLower().Contains(lowerFilter)
                               select item;

            return filteredList.ToList();
        }



        public static string GetFullPath(string fileName) // gibt den vollständigen pfad der datei zurück
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (File.Exists(fullPath))
            {
                return fullPath;
            }
            else
            {
                MessageBox.Show("Die Datei wurde nicht gefunden.");
                return null;
            }
        }


        // funktion die eine liste mit allen namen zurück gibt die die selbe kategorie haben wie das gesuchte wort

        public static List<string> ExtractStringListFromJsonCtaegory(string path, string filter) //funktioniert noch nicht wirklich (vielleicht sind auch die daten falsch)
        {
            List<string> resultList = new List<string>();

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Die JSON-Datei wurde nicht gefunden.", path);
                }

                using (StreamReader file = File.OpenText(path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JArray jsonArray = JArray.Load(reader);

                    foreach (JObject item in jsonArray.Children<JObject>())
                    {
                        // Filterprüfung nach Category
                        if (item.TryGetValue("Category", out JToken categoryValue) && categoryValue.ToString().Contains(filter))
                        {
                            // Extrahiere den Wert des "Name" Attributes
                            if (item.TryGetValue("Name", out JToken nameValue))
                            {
                                resultList.Add(nameValue.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Lesen der JSON-Datei: {ex.Message}");
                MessageBox.Show($"Fehler beim Lesen der JSON-Datei: {ex.Message}");
            }

            return resultList;
        }


        public static List<T> GetOneFullData<T>(string path, string name) // gibt alle daten zurück die den namen enthalten
        {
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path)).Where(x => x.GetType().GetProperty("Name").GetValue(x).ToString() == name).ToList();
        }

    }

}
