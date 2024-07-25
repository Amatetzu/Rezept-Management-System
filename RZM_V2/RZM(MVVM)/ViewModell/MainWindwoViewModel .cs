using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using RZM_MVVM_.View;
using GalaSoft.MvvmLight;
using System.Diagnostics;

namespace RZM_MVVM_.ViewModell
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Rezepte, Zutaten und Kategorien
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (Set(ref _searchText, value))
                {
                    UpdateList(); // Liste aktualisieren, wenn der Suchtext geändert wird
                }
            }
        }

        private string _selectetItemGenericList;
        public string SelectetItemGenericList
        {
            get { return _selectetItemGenericList; }
            set { Set(ref _selectetItemGenericList, value); }
        }

        private string _headerCategory;
        public string HeaderCategory
        {
            get { return _headerCategory; }
            set { _headerCategory = value; }
        }

        private string _headerMain;
        public string HeaderMain
        {
            get { return _headerMain; }
            set { Set(ref _headerMain, value); }
        }

        // Commands definieren
        public ICommand SearchCommand => new RelayCommand(UpdateList);
        public ICommand ShowCategorys => new RelayCommand(CategoryShow);
        public ICommand SchowRezepts => new RelayCommand(RezeptShow);
        public ICommand SchowIngredients => new RelayCommand(IngredientShow);
        public ICommand AddCommand => new RelayCommand(AddNewData);
        public ICommand EditCommand => new RelayCommand(OpenEditWindow);
        public ICommand DeleteCommand => new RelayCommand(DeletEntry);

        public ObservableCollection<string> GenericList { get; set; }

        public MainWindowViewModel()
        {
            GenericList = new ObservableCollection<string>();
            UpdateList();
            ((MainWindow)Application.Current.MainWindow).ListDoubleClick += ListDoubleClickHandler;
            HeaderMain = "Rezepte";
            Application.Current.MainWindow.Closed += (s, e) => Application.Current.Shutdown();
        }

        // Öffnet das entsprechende Fenster zum Hinzufügen neuer Daten
        private void AddNewData()
        {
            switch (HeaderMain)
            {
                case "Rezepte":
                    var addRezeptWindow = new AddRezeptWindow();
                    addRezeptWindow.Closed += ShowWindow_Closed;
                    addRezeptWindow.ShowDialog();
                    break;
                case "Zutaten":
                    var addZutatWindow = new AddZutatWindow();
                    addZutatWindow.Closed += ShowWindow_Closed;
                    addZutatWindow.ShowDialog();
                    break;
                case "Kategorien":
                    var addKategorieWindow = new AddKategorieWindow();
                    addKategorieWindow.Closed += ShowWindow_Closed;
                    addKategorieWindow.ShowDialog();
                    break;
                default:
                    MessageBox.Show("Kein Test");
                    break;
            }
        }

        // Event-Handler für Doppelklick auf ein ListView-Item
        private void ListDoubleClickHandler(object sender, EventArgs e)
        {
            Window showWindow;

            if (SelectetItemGenericList == null)
            {
                MessageBox.Show("Nicht ausgewählt");
                return;
            }

            switch (FullPath)
            {
                case var path when path == System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath):
                    showWindow = new ShowRezeptWindow();
                    Messenger.Default.Send(new UpdateHeaderMessage(SelectetItemGenericList));
                    break;
                case var path when path == System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath):
                    showWindow = new ShowZutatWindow();
                    Messenger.Default.Send(new UpdateZutatMessage(SelectetItemGenericList));
                    break;
                case var path when path == System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath):
                    showWindow = new ShowKategorieWindow();
                    Messenger.Default.Send(new UpdateKategorieMessage(SelectetItemGenericList));
                    break;
                default:
                    MessageBox.Show("Unbekannter Pfad");
                    return;
            }

            showWindow.Closed += ShowWindow_Closed;
            showWindow.ShowDialog();
        }

        // Wird ausgeführt, wenn ein modales Fenster geschlossen wird
        private void ShowWindow_Closed(object sender, EventArgs e)
        {
            UpdateList();
        }

        // Aktualisiert die Liste basierend auf dem Suchtext
        public void UpdateList()
        {
            if (SearchText == null)
            {
                SearchText = "";
            }

            var resultList = JsonUtils.ExtractStringListFromJson(FullPath, "Name");
            var filteredList = JsonUtils.FilterList(resultList, SearchText);
            GenericList.Clear();
            foreach (var item in filteredList)
            {
                GenericList.Add(item);
            }
        }

        // Setzt den Pfad auf Rezepte und aktualisiert die Liste
        public void RezeptShow()
        {
            FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);
            UpdateList();
            HeaderMain = "Rezepte";
        }

        // Setzt den Pfad auf Zutaten und aktualisiert die Liste
        public void IngredientShow()
        {
            HeaderMain = "Zutaten";
            FullPath = System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath);
            UpdateList();
        }

        // Setzt den Pfad auf Kategorien und aktualisiert die Liste
        public void CategoryShow()
        {
            FullPath = System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath);
            UpdateList();
            HeaderMain = "Kategorien";
        }

        // Löscht den ausgewählten Eintrag basierend auf der aktuellen Kategorie
        private void DeletEntry()
        {
            switch (HeaderMain)
            {
                case "Kategorien":
                    JsonUtils.DeleteJson<Kategorie>(FullPath, SelectetItemGenericList);
                    break;
                case "Zutaten":
                    JsonUtils.DeleteJson<Zutat>(FullPath, SelectetItemGenericList);
                    break;
                case "Rezepte":
                    JsonUtils.DeleteJson<Rezept>(FullPath, SelectetItemGenericList);
                    break;
                default:
                    MessageBox.Show("Nicht ausgewählt");
                    break;
            }

            UpdateList();
        }

        // Öffnet das entsprechende Fenster zum Bearbeiten eines Eintrags
        private void OpenEditWindow()
        {
            if (SelectetItemGenericList == null)
            {
                MessageBox.Show("Nicht ausgewählt");
                return;
            }

            switch (FullPath)
            {
                case var path when path == System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath):
                    OpenEditRezeptWindow(SelectetItemGenericList);
                    break;
                case var path when path == System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath):
                    OpenEditZutatWindow(SelectetItemGenericList);
                    break;
                case var path when path == System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath):
                    OpenEditKategorieWindow(SelectetItemGenericList);
                    break;
                default:
                    MessageBox.Show("Unbekannter Pfad");
                    break;
            }
        }

        private void OpenEditRezeptWindow(string selectItem)
        {
            var showWindow = new View.EditRezeptWindow
            {
                Owner = Application.Current.MainWindow
            };
            showWindow.Closed += ShowWindow_Closed;
            Messenger.Default.Send(new UpdateHeaderMessage(selectItem));
            showWindow.ShowDialog();
        }

        private void OpenEditZutatWindow(string selectItem)
        {
            var showWindow = new View.EditZutatWindow
            {
                Owner = Application.Current.MainWindow
            };
            showWindow.Closed += ShowWindow_Closed;
            Messenger.Default.Send(new UpdateZutatMessage(selectItem));
            showWindow.ShowDialog();
        }

        private void OpenEditKategorieWindow(string selectItem)
        {
            var showWindow = new View.EditKategorieWidow
            {
                Owner = Application.Current.MainWindow
            };
            showWindow.Closed += ShowWindow_Closed;
            Messenger.Default.Send(new UpdateKategorieMessage(selectItem));
            showWindow.ShowDialog();
        }
    }
}
