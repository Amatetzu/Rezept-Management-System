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
    internal class ShowKategorieWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Kategorien
        public string FullPath = Path.GetFullPath(ConstValues.KategorienJsonPath);

        // Kategorie-Objekt, das angezeigt wird
        private Kategorie _showKategorie;
        public Kategorie ShowKategorie
        {
            get { return _showKategorie; }
            set { Set(ref _showKategorie, value); }
        }

        // Alter Name der Kategorie (zum Vergleichen)
        public string oldName { get; set; }

        // Liste der Kategorien
        public List<Kategorie> kategories { get; set; }

        // Command zum Bearbeiten der Kategorie
        public ICommand EditKategorieCommand => new RelayCommand(EditData);

        public ShowKategorieWindowViewModel()
        {
            // Registrieren des Messengers zum Empfangen von Nachrichten
            Messenger.Default.Register<UpdateKategorieMessage>(this, HandleUpdateKategorieMessage);
            ShowKategorie = new Kategorie();
        }

        // Methode zum Öffnen des Bearbeitungsfensters für Kategorien
        private void EditData()
        {
            var showWindow = new EditKategorieWidow
            {
                Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive) // Setzt das aktive Fenster als Eigentümer
            };

            // Senden einer Nachricht mit dem alten Namen der Kategorie
            Messenger.Default.Send(new UpdateKategorieMessage(oldName));
            showWindow.Closed += ShowWindow_Closed;
            showWindow.ShowDialog();
        }

        // Handler für die Aktualisierung der Kategorie-Nachricht
        private void HandleUpdateKategorieMessage(UpdateKategorieMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.NewKategorie))
            {
                MessageBox.Show("Fehler: Ungültige Nachricht erhalten.");
                return;
            }

            // Setzt den neuen Namen der Kategorie
            ShowKategorie.Name = message.NewKategorie;
            oldName = message.NewKategorie;
            UpdateKategorie();
        }

        // Aktualisiert die Kategorie-Daten
        private void UpdateKategorie()
        {
            kategories = JsonUtils.GetOneFullData<Kategorie>(FullPath, oldName);
            if (kategories != null && kategories.Count > 0)
            {
                ShowKategorie = kategories[0];
            }
            else
            {
                MessageBox.Show("Fehler: Kategorie konnte nicht geladen werden.");
            }
        }

        // Event-Handler für das Schließen des Fensters
        private void ShowWindow_Closed(object sender, System.EventArgs e)
        {
            try
            {
                UpdateKategorie();
            }
            catch
            {
                // Schließt das aktuell aktive Fenster, wenn ein Fehler auftritt
                var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }
            }
        }
    }
}
