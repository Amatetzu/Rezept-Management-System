using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;

namespace RZM_MVVM_.ViewModell
{
    public class MainWindowViewModel : ViewModelBase
    {
        
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        private string _searchText;
        public string SearchText
            {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ICommand SearchCommand => new RelayCommand(execute => UpdateList());
        public ICommand ShowCategorys => new RelayCommand(execute => CategoryShow());
        public ICommand SchowRezepts => new RelayCommand(execute => RezeptShow());
        public ICommand SchowIngredients => new RelayCommand(execute => IngredientShow());
        public ICommand ItemDoubleClickCommand => new RelayCommand(execute => clearList());


        public ObservableCollection<string> GenericList { get; set; }


        public MainWindowViewModel()
        {
           
            GenericList = new ObservableCollection<string>();
            UpdateList();
            ((MainWindow)Application.Current.MainWindow).ListDoubleClick += ListDoubleClickHandler;

        }

        public int i = 0; //ich weis das das nicht schön ist aber es funktioniert bis jetzt
        private void ListDoubleClickHandler(object sender, EventArgs e)
        {
            
            if (i == 0) {
            View.ShowRezept showRezept = new View.ShowRezept();
            showRezept.Show();
                i++;
            }
            else
            {
                i = 0;
            }
            
        }
        public void UpdateList()
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            List<string> resultList = new List<string>();
            resultList = JsonUtils.ExtractStringListFromJson(FullPath, "Name");
            List<string> filteredList = new List<string>();
            filteredList=JsonUtils.FilterList(resultList, SearchText);
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
        public bool temp = false;
        public void clearList()
        {
           
            
        }

        EventHandler? ListDoubleClick (object sender, EventArgs e)
        {
            View.ShowRezept showRezept = new View.ShowRezept();
            showRezept.Show();
            return null;
        }



    }

}
