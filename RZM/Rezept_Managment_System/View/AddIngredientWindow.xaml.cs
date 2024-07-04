using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Rezept_Managment_System.userControlls;

namespace Rezept_Managment_System.View
{
    

   


    public partial class AddIngredientWindow : Window
    {
        
        
        public void SaveIngredient()
        {
            var zutat = new Zutat()
            {
                Name = name.UserInput,
                Einheit = einheit.UserInput,
                Menge = double.Parse(menge.UserInput),
                Allergene = ManageJson.SplitString(allergene.UserInput),
                EnergieKcal = double.Parse(energieKcal.UserInput),
                Kategorie = ManageJson.SplitString(kategorie.UserInput)
            };

            
            List<Zutat> zutaten = ManageJson.ReadJsonFile<List<Zutat>>(ManageJson.ZutatenPath);
            zutaten.Add(zutat);
            ManageJson.WriteJsonFile(ManageJson.ZutatenPath, zutaten);
        }

        public AddIngredientWindow()
        {
            InitializeComponent();
            
        }

        private void storage_Click(object sender, RoutedEventArgs e)
        {
            try {
                SaveIngredient();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Zutat: {ex.Message}");
            }
            

            
            
        }
    }
}
