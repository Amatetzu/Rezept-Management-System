using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RZM_MVVM_.Modell
{
    
       
    
    public class Zutat
    {
        public string Name { get; set; } = "Unbekannt";
        public string Einheit { get; set; } = "Stück";
        public double Menge { get; set; } = 0.0;
        public List<string> Allergene { get; set; } = new List<string>();
        public double EnergieKcal { get; set; } = 0.0;
        public List<string> KategorieNamen { get; set; } = new List<string>();
    }

    public class Rezept
    {
        public string Name { get; set; } = "Unbekanntes Rezept";
        public List<ZutatReferenz> Zutaten { get; set; } = new List<ZutatReferenz>();
        public string Zubereitung { get; set; } = "Keine Zubereitungsanweisung vorhanden";
        public List<string> Kategorien { get; set; }
        public List<string> Allergene { get; set; } = new List<string>();
        public int Zeit { get; set; } = 0; // in Minuten
    }

    public class ZutatReferenz
    {
        public string Name { get; set; }
        public double Menge { get; set; }
    }
    public class Kategorie
    {
        public string Name { get; set; }
        public string Beschreibung { get; set; }

    }
}
