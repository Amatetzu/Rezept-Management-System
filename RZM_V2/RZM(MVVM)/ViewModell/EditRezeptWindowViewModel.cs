using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class EditRezeptWindowViewModel : ViewModelBase
    {
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);
        public string OldName { get; set; }
        private string _rezeptName;
        public string RezeptName
        {
            get { return _rezeptName; }
            set { Set(ref _rezeptName, value); }
        }

        private string _rezeptZutaten;
        public string RezeptZutaten
        {
            get { return _rezeptZutaten; }
            set { Set(ref _rezeptZutaten, value); }
        }
        private string _rezeptZubereitung;
        public string RezeptZubereitung
        {
            get { return _rezeptZubereitung; }
            set { Set(ref _rezeptZubereitung, value); }
        }
        private string _kategorien;
        public string Kategorien
        {
            get { return _kategorien; }
            set { Set(ref _kategorien, value); }
        }
        private string _allergene;
        public string Allergene
        {
            get { return _allergene; }
            set { Set(ref _allergene, value); }
        }
        List<Rezept> rezepts { get; set; }
        Rezept rezept { get; set; }
        List<ZutatReferenz> zutats { get; set; }
        

        public EditRezeptWindowViewModel()
        {
            RezeptName = "Neues Rezept";
            Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);
            RezeptZubereitung = "Hier steht die Zubereitung";

            zutatenList = new ObservableCollection<string>();
            searchedZutatenList = new ObservableCollection<string>();



        }

        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            RezeptName = message.NewHeader;
            UpdateRezept();
            OldName = RezeptName;
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        public void UpdateRezept()
        {
            rezepts = JsonUtils.GetOneFullData<Rezept>(FullPath, RezeptName);

            rezept = rezepts[0];
            RezeptZubereitung = rezept.Zubereitung;
            zutats = rezept.Zutaten;
            for (int i = 0; i < zutats.Count; i++)
            {
                ZutatenList.Add(zutats[i].Name + "; " + zutats[i].Menge);
            }
            Kategorien = string.Join(", ", rezept.Kategorien);
            Allergene = string.Join(", ", rezept.Allergene);
            

        }


        public ICommand SaveCommand => new RelayCommand(SaveRezeptToJSON);
        public ICommand DeleteCommand => new RelayCommand(DeleteRezeptFromJSON);

        private void SaveRezeptToJSON()
        {
            rezept.Name= RezeptName;
            rezept.Zubereitung = RezeptZubereitung;
            rezept.Zutaten = JsonUtils.CombineZutaten(ZutatenList);
            rezept.Kategorien = Kategorien.Split(',').ToList();
            rezept.Allergene = Allergene.Split(',').ToList();
            JsonUtils.UpdateJson<Rezept>(FullPath, OldName, rezept);
            JsonUtils.SortJsonFileRezept(FullPath);
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (currentWindow != null)
            {
                
                currentWindow.Close();
            }

        }

        private void DeleteRezeptFromJSON()
        {
            MessageBoxResult result = MessageBox.Show("Wollen Sie das Rezept wirklich löschen?", "Löschen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                JsonUtils.DeleteJson<Rezept>(FullPath, OldName);
                Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                if (currentWindow != null)
                {
                    Window ownerWindow = currentWindow.Owner;
                    if (ownerWindow != null && ownerWindow.GetType() == typeof(View.ShowRezeptWindow))
                    {
                        ownerWindow.Close();
                    }
                    currentWindow.Close();
                }
            }
        }

    







    // beginn der zutaten logic

    // variablen für die zutaten
    private ObservableCollection<string> zutatenList;
        public ObservableCollection<string> ZutatenList
        {
            get { return zutatenList; }
            set { zutatenList = value; }
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

        //Command für Zutaten

        public ICommand ExpandButton => new RelayCommand(ExpandPopup);
        public ICommand DeleteZutat => new RelayCommand(DeleteZutatfromList);
        public ICommand AddZutat => new RelayCommand(AddZutatenToList);
        public ICommand SearchZutatenCommand => new RelayCommand(SearchZutaten);
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
            if (IsPopupOpen)
            {
                IsPopupOpen = false;
            }
            else
            {
                IsPopupOpen = true;
            }
        }// toggle für das popup


        private void AddZutatenToList()
        {
            if (ZutatenName != null && ZutatenMenge != null)
            {
                ZutatenList.Add(ZutatenName + "; " + ZutatenMenge);
                ZutatenName = "";
                ZutatenMenge ="";
            }

        }

        private void DeleteZutatfromList()
        {
            if (SelectedZutat != null)
            {
                ;
                ZutatenList.Remove(SelectedZutat);
            }
            else
            {
                MessageBox.Show("Keine Zutat ausgewählt.");
            }
        }

        private void SearchZutaten()
        {
            if (ZutatenName.Length > 0)
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

        // ende der zutaten logic

    }
}
