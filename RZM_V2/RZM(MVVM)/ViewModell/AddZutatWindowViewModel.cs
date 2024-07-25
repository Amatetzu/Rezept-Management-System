using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class AddZutatWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Zutaten
        public string FullPath = JsonUtils.GetFullPath(ConstValues.ZutatenJsonPath);

        // Liste von Zutaten (vermutlich aus einer anderen Quelle)
        public List<Zutat> zutats { get; set; }

        // Temporärer Name der Zutat vor der Bearbeitung
        private string oldName { get; set; }

        // Zutat, die bearbeitet wird
        private Zutat _editZutat;
        public Zutat EditZutat
        {
            get { return _editZutat; }
            set { Set(ref _editZutat, value); }
        }

        // Eingabe für Allergene als kommaseparierte Liste
        private string _allergenList;
        public string AllergenList
        {
            get { return _allergenList; }
            set
            {
                Set(ref _allergenList, value);
                // Konvertiere die kommagetrennte Liste in eine Liste von Allergenen
                EditZutat.Allergene = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }

        // Eingabe für Kategorien als kommaseparierte Liste
        private string _kategorieList;
        public string KategorieList
        {
            get { return _kategorieList; }
            set
            {
                Set(ref _kategorieList, value);
                // Konvertiere die kommagetrennte Liste in eine Liste von Kategorien
                EditZutat.KategorieNamen = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }

        // Command zum Aktualisieren der Zutat
        public ICommand UpdateZutatCommand => new RelayCommand(StoraData);

        // Command zum Löschen der Zutat
        public ICommand DeleteZutatCommand => new RelayCommand(ClearDate);

        // Konstruktor
        public AddZutatWindowViewModel()
        {
            EditZutat = new Zutat();
        }

       

        // Methode zum Speichern der Zutat
        private void StoraData()
        {
            UpdateZutat(FullPath, EditZutat);
        }

        // Methode zum Aktualisieren der Zutat in der JSON-Datei
        private void UpdateZutat(string path, Zutat data)
        {
            // Konvertiere die Eingaben in Listen und weise sie der Zutat zu
            data.KategorieNamen = KategorieList.Split(',').Select(s => s.Trim()).ToList();
            data.Allergene = AllergenList.Split(',').Select(s => s.Trim()).ToList();

            // Schreibe die Zutat in die JSON-Datei und sortiere sie
            JsonUtils.WriteJson(path, data);
            JsonUtils.SortJsonFileZutat(path);
            ClearDate();
        }

        // Methode zum Zurücksetzen der Eingaben und der Zutat
        private void ClearDate()
        {
            EditZutat = new Zutat();
            AllergenList = string.Empty;
            KategorieList = string.Empty;
        }
    }
}
