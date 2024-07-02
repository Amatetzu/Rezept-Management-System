using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rezept_Managment_System.utilities
{
    
    public class Zutat
    {
        public string Name { get; set; }
        public string Einheit { get; set; }
        public double Menge { get; set; }
        public List<string> Allergene { get; set; }
        public double EnergieKcal { get; set; }
        public List<string> Kategorie { get; set; }
    }

    public class Kategorie
    {
        public string Name { get; set; }
        public string Beschreibung { get; set; }
    }
}
