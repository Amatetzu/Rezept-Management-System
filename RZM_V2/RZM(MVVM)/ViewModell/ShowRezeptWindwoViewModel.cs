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

namespace RZM_MVVM_.ViewModell
{
    internal class ShowRezeptWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Rezepte
        public string FullPath = Path.GetFullPath(ConstValues.RezeptJsonPath);

        // Commands
        public ICommand TestCommand => new RelayCommand(TestFunktion);
        public ICommand EditCommand => new RelayCommand(EditRezept);

        // Name des Rezepts, das angezeigt werden soll
        public string RezeptName { get; set; }

        // Rezept-Objekt, das angezeigt wird
        private Rezept _showRezept;
        public Rezept ShowRezept
        {
            get { return _showRezept; }
            set { Set(ref _showRezept, value); }
        }

        // String-Darstellungen der Zutaten, Kategorien und Allergene des Rezepts
        private string _showRezeptZutaten;
        public string ShowRezeptZutaten
        {
            get { return _showRezeptZutaten; }
            set { Set(ref _showRezeptZutaten, value); }
        }

        private string _showRezeptKategorien;
        public string ShowRezeptKategorien
        {
            get { return _showRezeptKategorien; }
            set { Set(ref _showRezeptKategorien, value); }
        }

        private string _showRezeptAllergene;
        public string ShowRezeptAllergene
        {
            get { return _showRezeptAllergene; }
            set { Set(ref _showRezeptAllergene, value); }
        }

        // Liste der Rezepte
        public List<Rezept> Rezepte { get; set; }

        // Konstruktor
        public ShowRezeptWindowViewModel()
        {
            // Registrierung des Messengers zum Empfangen von Nachrichten
            Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);

            // Schließt die Anwendung, wenn das Hauptfenster geschlossen wird
            Application.Current.MainWindow.Closed += (s, e) => Application.Current.Shutdown();
        }

        // Testmethode zum Anzeigen einer Nachricht
        public void TestFunktion()
        {
            MessageBox.Show("Test");
        }

        // Handler für die Header-Update-Nachricht
        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            RezeptName = message.NewHeader;
            UpdateRezeptZubereitung();
        }

        // Methode zum Aktualisieren der Rezept-Daten
        private void UpdateRezeptZubereitung()
        {
            if (string.IsNullOrEmpty(RezeptName))
            {
                MessageBox.Show("RezeptName is not set.");
                return;
            }

            // Rezept aus der JSON-Datei laden
            Rezepte = JsonUtils.GetOneFullData<Rezept>(FullPath, RezeptName);

            if (Rezepte != null && Rezepte.Count > 0)
            {
                ShowRezept = Rezepte[0];
            }
            else
            {
                MessageBox.Show("No recipe found with the given name.");
                return;
            }

            // Zuweisung der Zutaten, Kategorien und Allergene als Strings
            ShowRezeptZutaten = string.Join("; ", ShowRezept.Zutaten.Select(z => z.Name + " " + z.Menge));
            ShowRezeptKategorien = string.Join("; ", ShowRezept.Kategorien);
            ShowRezeptAllergene = string.Join("; ", ShowRezept.Allergene);
        }

        // Aufräumen der Ressourcen
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        // Methode zum Öffnen des Bearbeitungsfensters für Rezepte
        public void EditRezept()
        {
            var editRezeptWindow = new View.EditRezeptWindow
            {
                Owner = Application.Current.Windows.OfType<View.ShowRezeptWindow>().FirstOrDefault() // Setzt das aktuelle Fenster als Eigentümer
            };

            // Senden einer Nachricht mit dem Namen des Rezepts
            Messenger.Default.Send(new UpdateHeaderMessage(ShowRezept.Name));
            editRezeptWindow.ShowDialog();
        }
    }
}
