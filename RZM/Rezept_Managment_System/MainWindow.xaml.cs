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

        private void addRezept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addZutat_Click(object sender, RoutedEventArgs e)
        {
         Ingredients ingredients = new Ingredients();
            ingredients.ShowDialog();
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.ShowDialog();
        }

        private void einkausliste_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}