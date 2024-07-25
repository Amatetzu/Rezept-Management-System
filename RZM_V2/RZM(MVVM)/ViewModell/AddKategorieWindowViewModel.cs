using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    // ViewModel für das Hinzufügen von Kategorien
    class AddKategorieWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Kategorien
        private readonly string _fullPath = JsonUtils.GetFullPath(ConstValues.KategorienJsonPath);

        // Kategorie-Objekt, das im ViewModel angezeigt wird
        private Kategorie _showKategorie;
        public Kategorie ShowKategorie
        {
            get { return _showKategorie; }
            set { Set(ref _showKategorie, value); }
        }

        // Command zum Speichern der Kategorie
        public ICommand StoreKategorie => new RelayCommand(StoreKategorieCommand);

        // Command-Methode zum Speichern der Kategorie
        private void StoreKategorieCommand()
        {
            if (ShowKategorie != null)
            {
                // Speichern der Kategorie in der JSON-Datei
                JsonUtils.WriteJson(_fullPath, ShowKategorie);
                // Zurücksetzen der Kategorie-Daten
                DeleteData();
                // Sortieren der JSON-Datei nach Namen
                JsonUtils.SortJsonFileKategorie(_fullPath);
            }
            else
            {
                // Fehlermeldung, wenn keine Kategorie eingegeben wurde
                MessageBox.Show("Bitte geben Sie eine Kategorie ein.");
            }
        }

        // Command zum Löschen der Kategorie
        public ICommand DeleteKategorie => new RelayCommand(DeleteData);

        // Methode zum Zurücksetzen der Kategorie-Daten
        private void DeleteData()
        {
            ShowKategorie = new Kategorie();
        }

        // Konstruktor
        public AddKategorieWindowViewModel()
        {
            // Initialisieren des Kategorie-Objekts
            ShowKategorie = new Kategorie();
        }
    }
}
