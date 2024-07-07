using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RZM_MVVM_.Modell
{
    public class ConstValues
    {
        public const string RezeptJsonPath = "Data/Rezepte.json";
        public const string ZutatenJsonPath = "Data/Zutaten.json";
        public const string KategorienJsonPath = "Data/Kategorien.json";
        public string FullRezeptJsonPath= System.IO.Path.GetFullPath(RezeptJsonPath);
    }

    
}
