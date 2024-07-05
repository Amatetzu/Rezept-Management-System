using Rezept_Managment_System.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rezept_Managment_System
{
    public partial class MainWindow : Window
    {
        

        public void AktiveSearch()
        {
            var names = utilities.ManageJson.AllSearch(utilities.ManageJson.KategorienPath, "Name");
            kategorilist.Clear();
            foreach (var name in names)
            {
                if (name.ToLower().Contains(searchBar.SearchText))
                {
                    kategorilist.Add((string)name);
                }
            }
        }

        public void LoadCategory()
        {
            var names = utilities.ManageJson.AllSearch(utilities.ManageJson.KategorienPath, "Name");
            kategorilist.Clear();
            foreach (var name in names)
            {
                kategorilist.Add((string)name);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // ListView for Kategorien
            Kategorilist = new ObservableCollection<string>();
            DataContext = this;
            // Alle Namen aus der JSON-Datei holen
            LoadCategory();
        }

        public ObservableCollection<string> Kategorilist
        {
            get { return kategorilist; }
            set { kategorilist = value; }
        }
        private ObservableCollection<string> kategorilist;
        

        

        private void rezept_Click(object sender, RoutedEventArgs e)
        {
            RezeptWindow rezeptWindow = new RezeptWindow();
            rezeptWindow.ShowDialog();
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
            // Implementiere die Logik für die Einkaufsliste hier
        }

        private void wochenPlan_Click(object sender, RoutedEventArgs e)
        {
            // Implementiere die Logik für den Wochenplan hier
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCategory();
        }

        private void searchBar_SearchButtonClicked(object sender, EventArgs e)
        {
            AktiveSearch();
        }

        private void searchBar_SearchFieldChanged(object sender, EventArgs e)
        {
            AktiveSearch();
        }

        private void categoryOverview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
