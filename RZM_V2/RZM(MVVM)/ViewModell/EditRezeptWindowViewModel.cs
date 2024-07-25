using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class EditRezeptWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Rezepte
        public string FullPath = Path.GetFullPath(ConstValues.RezeptJsonPath);

        // Der Name des Rezepts vor der Bearbeitung
        public string OldName { get; set; }

        // Rezept-Details
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

        // Listen für Rezepte und Zutaten
        List<Rezept> rezepts { get; set; }
        Rezept rezept { get; set; }
        List<ZutatReferenz> zutats { get; set; }

        // Initialisierung des ViewModels
        public EditRezeptWindowViewModel()
        {
            RezeptName = "Neues Rezept";
            Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);
            RezeptZubereitung = "Hier steht die Zubereitung";

            // Initialisierung der Listen für Zutaten
            ZutatenList = new ObservableCollection<string>();
            SearchedZutatenList = new ObservableCollection<string>();
        }

        // Behandelt Nachrichten zur Aktualisierung des Rezepts
        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            RezeptName = message.NewHeader;
            UpdateRezept();
            OldName = RezeptName;
        }

        // Aufräumen beim Schließen des ViewModels
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        // Aktualisiert das Rezept mit den aktuellen Daten
        public void UpdateRezept()
        {
            rezepts = JsonUtils.GetOneFullData<Rezept>(FullPath, RezeptName);

            if (rezepts.Any())
            {
                rezept = rezepts[0];
                RezeptZubereitung = rezept.Zubereitung;
                zutats = rezept.Zutaten;

                // Zutatenliste aktualisieren
                ZutatenList.Clear();
                foreach (var zutat in zutats)
                {
                    ZutatenList.Add($"{zutat.Name}; {zutat.Menge}");
                }

                Kategorien = string.Join(", ", rezept.Kategorien);
                Allergene = string.Join(", ", rezept.Allergene);
            }
        }

        // Commands für das Speichern und Löschen des Rezepts
        public ICommand SaveCommand => new RelayCommand(SaveRezeptToJSON);
        public ICommand DeleteCommand => new RelayCommand(DeleteRezeptFromJSON);

        // Speichert das Rezept in der JSON-Datei
        private void SaveRezeptToJSON()
        {
            rezept.Name = RezeptName;
            rezept.Zubereitung = RezeptZubereitung;
            rezept.Zutaten = JsonUtils.CombineZutaten(ZutatenList);
            rezept.Kategorien = Kategorien.Split(',').Select(s => s.Trim()).ToList();
            rezept.Allergene = Allergene.Split(',').Select(s => s.Trim()).ToList();

            JsonUtils.UpdateJson<Rezept>(FullPath, OldName, rezept);
            JsonUtils.SortJsonFileRezept(FullPath);

            // Schließe das aktuelle Fenster
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }

        // Löscht das Rezept aus der JSON-Datei
        private void DeleteRezeptFromJSON()
        {
            MessageBoxResult result = MessageBox.Show("Wollen Sie das Rezept wirklich löschen?", "Löschen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                JsonUtils.DeleteJson<Rezept>(FullPath, OldName);

                // Schließe das aktuelle Fenster
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

        // Beginn der Zutaten-Logik
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

        public string FullPathZutaten = Path.GetFullPath(ConstValues.ZutatenJsonPath);

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

        // Commands für die Zutaten
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

        // Öffnet oder schließt das Popup für Zutaten
        private void ExpandPopup()
        {
            IsPopupOpen = !IsPopupOpen;
        }

        // Fügt eine Zutat zur Liste hinzu
        private void AddZutatenToList()
        {
            if (!string.IsNullOrEmpty(ZutatenName) && !string.IsNullOrEmpty(ZutatenMenge))
            {
                ZutatenList.Add($"{ZutatenName}; {ZutatenMenge}");
                ZutatenName = "";
                ZutatenMenge = "";
            }
        }

        // Entfernt eine Zutat aus der Liste
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

        // Sucht nach Zutaten basierend auf dem eingegebenen Namen
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

        // Aktualisiert die Liste der gesuchten Zutaten
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

        // Verarbeitet den Klick auf eine gesuchte Zutat
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

        // Bearbeitet eine ausgewählte Zutat
        private void EditZutaten()
        {
            if (SelectedZutat != null)
            {
                List<string> tempzutat = SelectedZutat.Split(';').Select(s => s.Trim()).ToList();
                if (tempzutat.Count == 2)
                {
                    ZutatenName = tempzutat[0];
                    ZutatenMenge = tempzutat[1];
                    ZutatenList.Remove(SelectedZutat);
                }
            }
            else
            {
                MessageBox.Show("Keine Zutat ausgewählt.");
            }
        }
        // Ende der Zutaten-Logik
    }
}
