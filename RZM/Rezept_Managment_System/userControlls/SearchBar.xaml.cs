using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Rezept_Managment_System.userControlls
{
    public partial class SearchBar : UserControl, INotifyPropertyChanged
    {
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SearchButtonClicked; // Event für den Klick auf den Such-Button
        public event EventHandler SearchFieldChanged;

        public SearchBar()
        {
            InitializeComponent();
            DataContext = this; // Setze das DataContext auf diese Instanz der UserControl
        }

        private void searchfield_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchText = searchfield.Text; // Setze den Wert von SearchText auf den aktuellen Text des Textfelds
            placeholder.Visibility = string.IsNullOrEmpty(SearchText) ? Visibility.Visible : Visibility.Collapsed; // Zeige den Placeholder an, wenn der Text leer ist
            SearchFieldChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Löse das Event aus, wenn der Such-Button geklickt wird
            SearchButtonClicked?.Invoke(this, EventArgs.Empty);
            
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            searchfield.Clear();
            searchfield.Focus();
        }
    }
}
