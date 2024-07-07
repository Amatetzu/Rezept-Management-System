using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using RZM_MVVM_.View;
using GalaSoft.MvvmLight;
namespace RZM_MVVM_.ViewModell
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { Set(ref _searchText, value); }
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

        // Commands definieren
        public ICommand SearchCommand => new RelayCommand(UpdateList);
        public ICommand ShowCategorys => new RelayCommand(CategoryShow);
        public ICommand SchowRezepts => new RelayCommand(RezeptShow);
        public ICommand SchowIngredients => new RelayCommand(IngredientShow);

        public ObservableCollection<string> GenericList { get; set; }

        public MainWindowViewModel()
        {
            GenericList = new ObservableCollection<string>();
            UpdateList();
            ((MainWindow)Application.Current.MainWindow).ListDoubleClick += ListDoubleClickHandler;
        }

        // Event-Handler für Doppelklick auf ein ListView-Item
        private void ListDoubleClickHandler(object sender, EventArgs e)
        {
            

            // ShowCategoryWindow öffnen
            View.ShowCategoryWindow showCategory = new View.ShowCategoryWindow();
            Messenger.Default.Send(new UpdateHeaderMessage(SelectetItemGenericList));
            showCategory.ShowDialog();
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
        }

        public void IngredientShow()
        {
            FullPath = System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath);
            UpdateList();
        }

        public void CategoryShow()
        {
            FullPath = System.IO.Path.GetFullPath(ConstValues.KategorienJsonPath);
            UpdateList();
        }
    }
}
