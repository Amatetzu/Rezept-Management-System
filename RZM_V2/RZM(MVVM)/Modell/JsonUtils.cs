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
        /// <summary>
        /// Schreibt Daten in eine JSON-Datei.
        /// </summary>
        /// <param name="filePath">Pfad zur JSON-Datei.</param>
        /// <param name="dataToAppend">Daten, die hinzugefügt werden sollen.</param>
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

        /// <summary>
        /// Extrahiert eine Liste von Strings aus einer JSON-Datei basierend auf einem Attributnamen.
        /// </summary>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        /// <param name="attributeName">Name des Attributs, dessen Werte extrahiert werden sollen.</param>
        /// <returns>Liste von Strings.</returns>
        public static List<string> ExtractStringListFromJson(string path, string attributeName = "name")
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

        /// <summary>
        /// Filtert eine Liste von Strings basierend auf einem Filterstring.
        /// </summary>
        /// <param name="list">Liste von Strings, die gefiltert werden soll.</param>
        /// <param name="filter">Filterstring.</param>
        /// <returns>Gefilterte Liste von Strings.</returns>
        public static List<string> FilterList(List<string> list, string filter)
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

        /// <summary>
        /// Gibt den vollständigen Pfad einer Datei zurück.
        /// </summary>
        /// <param name="fileName">Name der Datei.</param>
        /// <returns>Vollständiger Pfad der Datei.</returns>
        public static string GetFullPath(string fileName)
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

        /// <summary>
        /// Extrahiert eine Liste von Strings aus einer JSON-Datei basierend auf einem Filter.
        /// </summary>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        /// <param name="filter">Filterstring.</param>
        /// <returns>Liste von Strings.</returns>
        public static List<string> ExtractStringListFromJsonCategory(string path, string filter)
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

        /// <summary>
        /// Gibt alle Daten zurück, die den angegebenen Namen enthalten.
        /// </summary>
        /// <typeparam name="T">Datentyp.</typeparam>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        /// <param name="name">Name zum Filtern.</param>
        /// <returns>Liste der gefilterten Daten.</returns>
        public static List<T> GetOneFullData<T>(string path, string name)
        {
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path))
                   .Where(x => x.GetType().GetProperty("Name").GetValue(x).ToString() == name)
                   .ToList();
        }

        /// <summary>
        /// Aktualisiert Daten in einer JSON-Datei.
        /// </summary>
        /// <typeparam name="T">Datentyp.</typeparam>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        /// <param name="name">Name des zu aktualisierenden Elements.</param>
        /// <param name="newData">Neue Daten.</param>
        public static void UpdateJson<T>(string path, string name, T newData)
        {
            List<T> data = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
            data.RemoveAll(x => x.GetType().GetProperty("Name").GetValue(x).ToString() == name);
            data.Add(newData);
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// Löscht Daten aus einer JSON-Datei.
        /// </summary>
        /// <typeparam name="T">Datentyp.</typeparam>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        /// <param name="name">Name des zu löschenden Elements.</param>
        public static void DeleteJson<T>(string path, string name) where T : class
        {
            List<T> data = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
            data.RemoveAll(x => x.GetType().GetProperty("Name").GetValue(x).ToString() == name);
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// Kombiniert Zutaten zu einer Liste von ZutatReferenz-Objekten.
        /// </summary>
        /// <param name="dataList">Liste der Zutaten als Strings.</param>
        /// <returns>Liste von ZutatReferenz-Objekten.</returns>
        public static List<ZutatReferenz> CombineZutaten(ObservableCollection<string> dataList)
        {
            List<ZutatReferenz> resultList = new List<ZutatReferenz>();
            for (int i = 0; i < dataList.Count; i++)
            {
                ZutatReferenz data = new ZutatReferenz
                {
                    Name = dataList[i].Split(';')[0],
                    Menge = double.Parse(dataList[i].Split(';')[1])
                };
                resultList.Add(data);
            }
            return resultList;
        }

        /// <summary>
        /// Sortiert die JSON-Datei für Rezepte nach dem Namen.
        /// </summary>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        public static void SortJsonFileRezept(string path)
        {
            List<Rezept> data = JsonConvert.DeserializeObject<List<Rezept>>(File.ReadAllText(path));
            data = data.OrderBy(x => x.Name).ToList();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// Sortiert die JSON-Datei für Zutaten nach dem Namen.
        /// </summary>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        public static void SortJsonFileZutat(string path)
        {
            List<Zutat> data = JsonConvert.DeserializeObject<List<Zutat>>(File.ReadAllText(path));
            data = data.OrderBy(x => x.Name).ToList();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// Sortiert die JSON-Datei für Kategorien nach dem Namen.
        /// </summary>
        /// <param name="path">Pfad zur JSON-Datei.</param>
        public static void SortJsonFileKategorie(string path)
        {
            List<Kategorie> data = JsonConvert.DeserializeObject<List<Kategorie>>(File.ReadAllText(path));
            data = data.OrderBy(x => x.Name).ToList();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
