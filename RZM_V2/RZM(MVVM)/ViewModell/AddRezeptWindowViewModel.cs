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
        // Pfad zur JSON-Datei für Rezepte
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        // Rezept-Objekt, das im ViewModel angezeigt wird
        private Rezept _newRezept;
        public Rezept NewRezept
        {
            get { return _newRezept; }
            set { Set(ref _newRezept, value); }
        }

        // Kategorien-Eingabe als String
        private string _newKategorien;
        public string NewKategorien
        {
            get { return _newKategorien; }
            set { Set(ref _newKategorien, value); }
        }

        // Allergene-Eingabe als String
        private string _newAllergene;
        public string NewAllergene
        {
            get { return _newAllergene; }
            set { Set(ref _newAllergene, value); }
        }

        // Liste von Rezepten und Zutatenreferenzen
        public List<Rezept> rezepts { get; set; }
        public List<ZutatReferenz> zutats { get; set; }

        // ObservableCollection für Zutatenliste
        private ObservableCollection<string> zutatenList;
        public ObservableCollection<string> ZutatenList
        {
            get { return zutatenList; }
            set { Set(ref zutatenList, value); }
        }

        // Name der Zutat, die hinzugefügt werden soll
        private string _zutatenName;
        public string ZutatenName
        {
            get => _zutatenName;
            set
            {
                if (Set(ref _zutatenName, value))
                {
                    SearchZutaten(); // Suche nach passenden Zutaten bei Änderung
                }
            }
        }

        // Menge der Zutat, die hinzugefügt werden soll
        private string _zutatenMenge;
        public string ZutatenMenge
        {
            get { return _zutatenMenge; }
            set { Set(ref _zutatenMenge, value); }
        }

        // Ausgewählte Zutat aus der Liste
        private string _selectedZutat;
        public string SelectedZutat
        {
            get { return _selectedZutat; }
            set { Set(ref _selectedZutat, value); }
        }

        // Pfad zur JSON-Datei für Zutaten
        public string FullPathZutaten = System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath);

        // ObservableCollection für die Suchergebnisse der Zutaten
        private ObservableCollection<string> searchedZutatenList;
        public ObservableCollection<string> SearchedZutatenList
        {
            get { return searchedZutatenList; }
            set { Set(ref searchedZutatenList, value); }
        }

        // Status, ob das Such-Popup geöffnet ist
        private bool _isSearchPopupOpen;
        public bool IsSearchPopupOpen
        {
            get { return _isSearchPopupOpen; }
            set { Set(ref _isSearchPopupOpen, value); }
        }

        // Ausgewählte Zutat aus der Suchergebnis-Liste
        private string _selectedSearchedZutat;
        public string SelectedSearchedZutat
        {
            get { return _selectedSearchedZutat; }
            set { Set(ref _selectedSearchedZutat, value); }
        }

        // Konstruktor
        public AddRezeptWindowViewModel()
        {
            ZutatenList = new ObservableCollection<string>();
            SearchedZutatenList = new ObservableCollection<string>();
            NewRezept = new Rezept();
        }

        // Command zum Speichern des Rezepts
        public ICommand SaveCommand => new RelayCommand(SaveRezeptToJSON);

        // Command zum Zurücksetzen des Rezepts
        public ICommand ClearCommand => new RelayCommand(ClearRezept);

        // Methode zum Speichern des Rezepts in der JSON-Datei
        private void SaveRezeptToJSON()
        {
            if (string.IsNullOrWhiteSpace(NewRezept.Name))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.");
            }
            else
            {
                // Kombinieren der Zutaten und Zuweisen der Kategorien und Allergene
                NewRezept.Zutaten = JsonUtils.CombineZutaten(ZutatenList);
                NewRezept.Kategorien = NewKategorien.Split(',').ToList();
                NewRezept.Allergene = NewAllergene.Split(',').ToList();
                // Speichern des Rezepts
                JsonUtils.WriteJson(FullPath, NewRezept);
                // Zurücksetzen der Eingaben
                ClearRezept();
                // Sortieren der JSON-Datei
                JsonUtils.SortJsonFileRezept(FullPath);
            }
        }

        // Methode zum Zurücksetzen der Rezept-Eingaben
        private void ClearRezept()
        {
            NewRezept = new Rezept();
            NewKategorien = string.Empty;
            NewAllergene = string.Empty;
            ZutatenList.Clear();
        }

        // Beginn der Zutaten-Logik

        // Command zum Öffnen des Popup-Fensters für Zutaten
        public ICommand ExpandButton => new RelayCommand(ExpandPopup);

        // Command zum Löschen einer Zutat aus der Liste
        public ICommand DeleteZutat => new RelayCommand(DeleteZutatfromList);

        // Command zum Hinzufügen einer Zutat zur Liste
        public ICommand AddZutat => new RelayCommand(AddZutatenToList);

        // Command zum Bearbeiten einer Zutat
        public ICommand EditZutat => new RelayCommand(EditZutaten);

        // Command zum Verarbeiten von Klicks auf Listeneinträge
        public ICommand ItemClickCommand => new RelayCommand(OnItemClick);

        // Status, ob das Popup-Fenster für Zutaten geöffnet ist
        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set { Set(ref _isPopupOpen, value); }
        }

        // Methode zum Öffnen oder Schließen des Popup-Fensters für Zutaten
        private void ExpandPopup()
        {
            IsPopupOpen = !IsPopupOpen;
        }

        // Methode zum Hinzufügen einer Zutat zur Liste
        private void AddZutatenToList()
        {
            if (!string.IsNullOrEmpty(ZutatenName) && !string.IsNullOrEmpty(ZutatenMenge))
            {
                ZutatenList.Add($"{ZutatenName}; {ZutatenMenge}");
                ZutatenName = string.Empty;
                ZutatenMenge = string.Empty;
            }
        }

        // Methode zum Löschen einer Zutat aus der Liste
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

        // Methode zum Suchen nach Zutaten basierend auf dem eingegebenen Namen
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

        // Methode zum Aktualisieren der Suchergebnisse für Zutaten
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

        // Methode zum Verarbeiten des Klicks auf eine Zutat in der Suchergebnis-Liste
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

        // Methode zum Bearbeiten einer ausgewählten Zutat
        private void EditZutaten()
        {
            if (SelectedZutat != null)
            {
                List<string> tempzutat = SelectedZutat.Split(';').ToList();
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
