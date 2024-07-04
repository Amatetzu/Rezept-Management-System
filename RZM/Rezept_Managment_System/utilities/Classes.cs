using System;
using System.Collections.Generic;

namespace Rezept_Managment_System.utilities
{
    public class Zutat
    {
        public string Name { get; set; } = "Unbekannt";
        public string Einheit { get; set; } = "Stück";
        public double Menge { get; set; } = 0.0;
        public List<string> Allergene { get; set; } = new List<string>();
        public double EnergieKcal { get; set; } = 0.0;
        public List<string> Kategorie { get; set; } = new List<string>();
    }

    public class Kategorie
    {
        public string Name { get; set; } = "Allgemein";
        public string Beschreibung { get; set; } = "Keine Beschreibung verfügbar";
    }

    public class Rezept
    {
        public string Name { get; set; } = "Unbekanntes Rezept";
        public List<string> Zutaten { get; set; } = new List<string> { ""};
        public string Zubereitung { get; set; } = "Keine Zubereitungsanweisung vorhanden";
        public List<string> Kategorien { get; set; } = new List<string> { "Allgemein" };
        public double EnergieKcal { get; set; } = 0.0;
        public List<string> Allergene { get; set; } = new List<string>();
        public int Zeit { get; set; } = 0; // in Minuten
    }
}
