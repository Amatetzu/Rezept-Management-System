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

namespace Rezept_Managment_System.View
{
    

   


    public partial class AddIngredientWindow : Window
    {
        private List<Zutat> zutatenListe = new List<Zutat>();
        
        

        public AddIngredientWindow()
        {
            InitializeComponent();
            
        }

        private void storage_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
