using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rezept_Managment_System.View;

namespace Rezept_Managment_System
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void searchBar_SearchButtonClicked(object sender, EventArgs e)
        {
            kategorilist.Items.Add(new ListViewItem() { Content = searchBar.SearchText });
        }

        private void rezept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void zutat_Click(object sender, RoutedEventArgs e)
        {
         Ingredients ingredients = new Ingredients();
            ingredients.ShowDialog();
        }

        private void category_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow();
            categoryWindow.ShowDialog();
        }

        private void einkausliste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void wochenPlan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}