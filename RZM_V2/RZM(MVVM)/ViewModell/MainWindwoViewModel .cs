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
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (Set(ref _searchText, value))
                {
                    
                    UpdateList();
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
            set
            { _headerCategory = value; }
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

        public ObservableCollection<string> GenericList { get; set; }

        public MainWindowViewModel()
        {
            GenericList = new ObservableCollection<string>();
            UpdateList();
            ((MainWindow)Application.Current.MainWindow).ListDoubleClick += ListDoubleClickHandler;
            HeaderMain = "Rezepte";
            Application.Current.MainWindow.Closed += (s, e) => Application.Current.Shutdown();
        }

        private void AddNewData()
        {
           
            switch (HeaderMain)
            {
                case "Rezepte":
                   AddRezeptWindow showWindow = new AddRezeptWindow();
                    showWindow.Closed += ShowWindow_Closed;
                    showWindow.ShowDialog();
                    break;
                case "Zutaten":
                    AddZutatWindow showWindow1 = new AddZutatWindow();
                    showWindow1.Closed += ShowWindow_Closed;
                    showWindow1.ShowDialog();
                    break;
                
                case "Kategorien":
                    AddKategorieWindow showWindow2 = new AddKategorieWindow();
                    showWindow2.Closed += ShowWindow_Closed;
                    showWindow2.ShowDialog();
                    break;
                
                default:
                    MessageBox.Show("kein TEst");
                    break;

              
            }
           
        }

        

        // Event-Handler für Doppelklick auf ein ListView-Item
        private void ListDoubleClickHandler(object sender, EventArgs e)
        {
            var showWindow = new Window();
            if (SelectetItemGenericList == null)
            {
                MessageBox.Show("Nichtsausgewählt");
                return;
            }
            else
            {
                if (FullPath == System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath))
                {
                    showWindow = new ShowRezeptWindow();
                    Messenger.Default.Send(new UpdateHeaderMessage(SelectetItemGenericList));

                }
                else if (FullPath == System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath))
                {
                    showWindow = new ShowZutatWindow();
                    Messenger.Default.Send(new UpdateZutatMessage(SelectetItemGenericList));
                }
                else if (FullPath == System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath))
                {
                    showWindow = new ShowKategorieWindow();
                    Messenger.Default.Send(new UpdateKategorieMessage(SelectetItemGenericList));

                }
                showWindow.Closed += ShowWindow_Closed; // Event-Handler hinzufügen

                showWindow.ShowDialog();
            }
        }

        private void ShowWindow_Closed(object sender, EventArgs e)
        {
           
            // de, der ausgeführt wird, wenn das modale Fenster geschlossen wird
            UpdateList();
          
        }

        public void UpdateList()
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            List<string> resultList = JsonUtils.ExtractStringListFromJson(FullPath, "Name");
            List<string> filteredList = JsonUtils.FilterList(resultList, SearchText);
            GenericList.Clear();
            foreach (var item in filteredList)
            {
                GenericList.Add(item);
            }
        }

        public void RezeptShow()
        {
            FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);
            UpdateList();
            HeaderMain = "Rezepte";
        }

        public void IngredientShow()
        {
            HeaderMain = "Zutaten";
            FullPath = System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath);
            UpdateList();

        }

        public void CategoryShow()
        {
            FullPath = System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath);
            UpdateList();
            HeaderMain = "Kategorien";
        }


        //COmands Edit ADD Delet
        public ICommand EditCommand => new RelayCommand(OpenEditWindow);
        public ICommand DeleteCommand => new RelayCommand(DeletEntry);
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

                default :
                    MessageBox.Show("nichtsausgewählt");
                    break;

            }
            
            UpdateList();
        }
        public void OpenEditWindow()
        {
            if (SelectetItemGenericList != null)
            {
                if (FullPath == System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath))
                {

                    OpenEditRezeptWindow(SelectetItemGenericList);
                }
                else if (FullPath == System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath))
                {
                    OpenEditZutatWindow(SelectetItemGenericList);
                }
                else if (FullPath == System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath))
                {
                    OpenEditKategorieWindow(SelectetItemGenericList);
                }

            }
            else
            {
                MessageBox.Show("Nichtsausgewählt");
            }

        }

        private void OpenEditRezeptWindow(string selectItem)
        {
            View.EditRezeptWindow showWindow = new View.EditRezeptWindow();
            showWindow.Owner = Application.Current.MainWindow; // Setzt das Hauptfenster als Eigentümer
            showWindow.Closed += ShowWindow_Closed;
            Messenger.Default.Send(new UpdateHeaderMessage(selectItem));
            showWindow.ShowDialog();
        }
        private void OpenEditZutatWindow(string selectItem)
        {
            View.EditZutatWindow showWindow = new View.EditZutatWindow();
            showWindow.Owner = Application.Current.MainWindow; // Setzt das Hauptfenster als Eigentümer
            showWindow.Closed += ShowWindow_Closed;
            Messenger.Default.Send(new UpdateHeaderMessage(selectItem));
            showWindow.ShowDialog();
        }
        private void OpenEditKategorieWindow(string selectItem)
        {
            EditKategorieWidow showWindow = new View.EditKategorieWidow();
            showWindow.Owner = Application.Current.MainWindow; // Setzt das Hauptfenster als Eigentümer
            showWindow.Closed += ShowWindow_Closed;
            Messenger.Default.Send(new UpdateKategorieMessage(selectItem));
            showWindow.ShowDialog();
        }


    }
}
