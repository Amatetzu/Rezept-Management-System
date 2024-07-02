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

namespace Rezept_Managment_System.View
{
    /// <summary>
    /// Interaktionslogik für Ingredients.xaml
    /// </summary>
    public partial class Ingredients : Window
    {
        public Ingredients()
        {
            InitializeComponent();
        }

        private void storage_Click(object sender, RoutedEventArgs e)
        {
            AddIngredientWindow addIngredientWindow = new AddIngredientWindow();
            addIngredientWindow.ShowDialog();
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
