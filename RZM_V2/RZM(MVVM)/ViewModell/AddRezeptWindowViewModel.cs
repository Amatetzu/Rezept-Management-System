using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class AddRezeptWindowViewModel : ViewModelBase
    {
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        private Rezept _newRezept;
        public Rezept NewRezept
        {
            get { return _newRezept; }
            set { Set(ref _newRezept, value); }
        }

        private string _newKategorien;
        public string NewKategorien
        {
            get { return _newKategorien; }
            set { Set(ref _newKategorien, value); }
        }

        private string _newAllergene;
        public string NewAllergene
        {
            get { return _newAllergene; }
            set { Set(ref _newAllergene, value); }
        }

        public List<Rezept> rezepts { get; set; }
        public List<ZutatReferenz> zutats { get; set; }

        private ObservableCollection<string> zutatenList;
        public ObservableCollection<string> ZutatenList
        {
            get { return zutatenList; }
            set { Set(ref zutatenList, value); }
        }

        private string _zutatenName;
        public string ZutatenName
        {
            get => _zutatenName;
            set
            {
                if (Set(ref _zutatenName, value))
                {
                    SearchZutaten();
                }
            }
        }

        private string _zutatenMenge;
        public string ZutatenMenge
        {
            get { return _zutatenMenge; }
            set { Set(ref _zutatenMenge, value); }
        }

        private string _selectedZutat;
        public string SelectedZutat
        {
            get { return _selectedZutat; }
            set { Set(ref _selectedZutat, value); }
        }

        public string FullPathZutaten = System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath);

        private ObservableCollection<string> searchedZutatenList;
        public ObservableCollection<string> SearchedZutatenList
        {
            get { return searchedZutatenList; }
            set { Set(ref searchedZutatenList, value); }
        }

        private bool _isSearchPopupOpen;
        public bool IsSearchPopupOpen
        {
            get { return _isSearchPopupOpen; }
            set { Set(ref _isSearchPopupOpen, value); }
        }

        private string _selectedSearchedZutat;
        public string SelectedSearchedZutat
        {
            get { return _selectedSearchedZutat; }
            set { Set(ref _selectedSearchedZutat, value); }
        }

        public AddRezeptWindowViewModel()
        {
            ZutatenList = new ObservableCollection<string>();
            SearchedZutatenList = new ObservableCollection<string>();
            NewRezept = new Rezept();
        }

        public ICommand SaveCommand => new RelayCommand(SaveRezeptToJSON);
        public ICommand ClearCommand => new RelayCommand(ClearRezept);

        private void SaveRezeptToJSON()
        {
            if (string.IsNullOrWhiteSpace(NewRezept.Name))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.");
            }
            else
            {
                NewRezept.Zutaten = JsonUtils.CombineZutaten(ZutatenList);
                NewRezept.Kategorien = NewKategorien.Split(',').ToList();
                NewRezept.Allergene = NewAllergene.Split(',').ToList();
                JsonUtils.WriteJson(FullPath, NewRezept);
                ClearRezept();
                JsonUtils.SortJsonFileRezept(FullPath);
            }
        }

        private void ClearRezept()
        {
            NewRezept = new Rezept();
            NewKategorien = "";
            NewAllergene = "";
            ZutatenList.Clear();
        }

        // Beginn der Zutaten-Logik

        public ICommand ExpandButton => new RelayCommand(ExpandPopup);
        public ICommand DeleteZutat => new RelayCommand(DeleteZutatfromList);
        public ICommand AddZutat => new RelayCommand(AddZutatenToList);
        public ICommand ItemClickCommand => new RelayCommand(OnItemClick);
        public ICommand EditZutat => new RelayCommand(EditZutaten);

        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set { Set(ref _isPopupOpen, value); }
        }

        private void ExpandPopup()
        {
            IsPopupOpen = !IsPopupOpen;
        }

        private void AddZutatenToList()
        {
            if (!string.IsNullOrEmpty(ZutatenName) && !string.IsNullOrEmpty(ZutatenMenge))
            {
                ZutatenList.Add($"{ZutatenName}; {ZutatenMenge}");
                ZutatenName = "";
                ZutatenMenge = "";
            }
        }

        private void DeleteZutatfromList()
        {
            if (SelectedZutat != null)
            {
                ZutatenList.Remove(SelectedZutat);
            }
            else
            {
                MessageBox.Show("Keine Zutat ausgewählt.");
            }
        }

        private void SearchZutaten()
        {
            if (!string.IsNullOrEmpty(ZutatenName))
            {
                IsSearchPopupOpen = true;
                UpdateZutaten();
                if (SearchedZutatenList.Count == 0)
                {
                    IsSearchPopupOpen = false;
                }
            }
            else
            {
                IsSearchPopupOpen = false;
            }
        }

        private void UpdateZutaten()
        {
            List<string> resultList = JsonUtils.ExtractStringListFromJson(FullPathZutaten, "Name");
            List<string> filteredList = JsonUtils.FilterList(resultList, ZutatenName);
            SearchedZutatenList.Clear();
            foreach (var item in filteredList)
            {
                SearchedZutatenList.Add(item);
            }
        }

        private void OnItemClick()
        {
            if (SelectedSearchedZutat != null)
            {
                ZutatenName = SelectedSearchedZutat;
                IsSearchPopupOpen = false;
            }
            else
            {
                MessageBox.Show("Keine Zutat ausgewählt.");
            }
        }

        private void EditZutaten()
        {
            if (SelectedZutat != null)
            {
                List<string> tempzutat = SelectedZutat.Split(' ').ToList();
                ZutatenName = tempzutat[0];
                ZutatenMenge = tempzutat[1];
                ZutatenList.Remove(SelectedZutat);
            }
            else
            {
                MessageBox.Show("Keine Zutat ausgewählt.");
            }
        }

        // Ende der Zutaten-Logik
    }
}
