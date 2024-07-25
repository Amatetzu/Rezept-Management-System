using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RZM_MVVM_.View;
using RZM_MVVM_.ViewModell;

namespace RZM_MVVM_.ViewModell
{
    /// <summary>
    /// ViewModel für das Bearbeiten von Kategorien.
    /// </summary>
    class EditKategorieWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Kategorien
        public string FullPath = Path.GetFullPath(ConstValues.KategorienJsonPath);

        // Die aktuell angezeigte Kategorie
        private Kategorie _showKategorie;
        public Kategorie ShowKategorie
        {
            get { return _showKategorie; }
            set { Set(ref _showKategorie, value); }
        }

        // Der Name der Kategorie vor der Bearbeitung
        public string OldName { get; set; }

        // Liste der Kategorien (vermutlich für interne Verwendung)
        public List<Kategorie> Kategorien { get; set; }

        // Command zum Speichern der bearbeiteten Kategorie
        public ICommand SaveKategorieCommand => new RelayCommand(SaveKategorie);

        // Command zum Löschen der Kategorie
        public ICommand DeleteKategorieCommand => new RelayCommand(DeleteKategorie);

        // Konstruktor
        public EditKategorieWindowViewModel()
        {
            // Registrierung des Messengers zur Verarbeitung von Update-Nachrichten
            Messenger.Default.Register<UpdateKategorieMessage>(this, HandleUpdateKategorieMessage);

            // Initialisierung der Kategorie
            ShowKategorie = new Kategorie();
        }

        // Methode zum Speichern der Kategorie
        private void SaveKategorie()
        {
            // Aktualisiere die Kategorie in der JSON-Datei
            JsonUtils.UpdateJson<Kategorie>(FullPath, OldName, ShowKategorie);

            // Schließe das aktuelle Fenster
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }

        // Methode zum Löschen der Kategorie
        private void DeleteKategorie()
        {
            // Lösche die Kategorie aus der JSON-Datei
            JsonUtils.DeleteJson<Kategorie>(FullPath, OldName);

            // Schließe das aktuelle Fenster
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }

        // Methode zur Verarbeitung der Update-Nachricht für eine Kategorie
        private void HandleUpdateKategorieMessage(UpdateKategorieMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.NewKategorie))
            {
                MessageBox.Show("Fehler: Ungültige Nachricht erhalten.");
                return;
            }

            // Setze den neuen Namen der Kategorie
            ShowKategorie.Name = message.NewKategorie;
            OldName = message.NewKategorie;

            // Aktualisiere die Kategorie
            UpdateKategorie();
        }

        // Methode zur Aktualisierung der Kategorie
        private void UpdateKategorie()
        {
            // Hole die vollständigen Daten für die Kategorie
            Kategorien = JsonUtils.GetOneFullData<Kategorie>(FullPath, OldName);

            // Setze die aktualisierte Kategorie
            if (Kategorien.Any())
            {
                ShowKategorie = Kategorien[0];
            }
        }
    }
}
