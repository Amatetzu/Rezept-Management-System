using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Rezept_Managment_System.utilities;

namespace Rezept_Managment_System.View
{
    /// <summary>
    /// Interaktionslogik für AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        public AddCategoryWindow()
        {
            InitializeComponent();
            string fullPath = ManageJson.GetfullPath("Data/Kategorien.json");
        }

        private void add_GenericButtonClicked(object sender, EventArgs e)
        {
            var kategorie = new Kategorie()
            {
                Name = name.UserInput,
                Beschreibung = description.UserInput
            };
            string fullPath = ManageJson.GetfullPath("Data/Kategorien.json");

            List<Kategorie> kategorien = ManageJson.ReadJsonFile<List<Kategorie>>(fullPath);
            kategorien.Add(kategorie);
            ManageJson.WriteJsonFile(fullPath, kategorien);
        }
    }
}
