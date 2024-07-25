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
    internal class EditZutatWindowViewModel : ViewModelBase
    {
        private string oldName;
        private Zutat _editZutat;

        // Pfad zur JSON-Datei für Zutaten
        public string FullPath = Path.GetFullPath(ConstValues.ZutatenJsonPath);
        public List<Zutat> zutats { get; set; }

        private string _allergenList;
        private string _kategorieList;

        // Eigenschaften für Allergene und Kategorien, die an die UI gebunden sind
        public string AllergenList
        {
            get { return _allergenList; }
            set
            {
                Set(ref _allergenList, value);
                if (EditZutat != null)
                {
                    EditZutat.Allergene = value.Split(',').Select(s => s.Trim()).ToList();
                }
            }
        }

        public string KategorieList
        {
            get { return _kategorieList; }
            set
            {
                Set(ref _kategorieList, value);
                if (EditZutat != null)
                {
                    EditZutat.KategorieNamen = value.Split(',').Select(s => s.Trim()).ToList();
                }
            }
        }

        // Commands für das Speichern und Löschen der Zutat
        public ICommand UpdateZutatCommand => new RelayCommand(StoraData);
        public ICommand DeleteZutatCommand => new RelayCommand(DeleteZutat);

        // Konstruktor
        public EditZutatWindowViewModel()
        {
            Messenger.Default.Register<UpdateZutatMessage>(this, HandleUpdateZutatMessage);
            EditZutat = new Zutat();
        }

        // Behandelt Nachrichten zur Aktualisierung der Zutat
        private void HandleUpdateZutatMessage(UpdateZutatMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.NewZutat))
            {
                MessageBox.Show("Fehler: Ungültige Nachricht erhalten.");
                return;
            }

            if (EditZutat == null)
            {
                MessageBox.Show("Fehler: EditZutat ist null.");
                return;
            }

            EditZutat.Name = message.NewZutat;
            oldName = EditZutat.Name;
            UpdateList();
        }

        // Speichert die Zutat in der JSON-Datei
        private void StoraData()
        {
            UpdateZutat(FullPath, oldName);
        }

        // Löscht die Zutat aus der JSON-Datei
        private void DeleteZutat()
        {
            MessageBoxResult result = MessageBox.Show("Wollen Sie die Zutat wirklich löschen?", "Löschen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                JsonUtils.DeleteJson<Zutat>(FullPath, oldName);

                // Schließe das aktuelle Fenster
                Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                currentWindow?.Close();
            }
        }

        // Aktualisiert die Zutat-Liste mit den Daten aus der JSON-Datei
        private void UpdateList()
        {
            zutats = JsonUtils.GetOneFullData<Zutat>(FullPath, oldName);
            if (zutats == null || !zutats.Any())
            {
                MessageBox.Show("Fehler: Zutat konnte nicht geladen werden.");
                return;
            }

            EditZutat = zutats[0];
            AllergenList = string.Join(", ", EditZutat.Allergene);
            KategorieList = string.Join(", ", EditZutat.KategorieNamen);
        }

        // Eigenschaft für die zu bearbeitende Zutat
        public Zutat EditZutat
        {
            get { return _editZutat; }
            set { Set(ref _editZutat, value); }
        }

        // Aufräumen beim Schließen des ViewModels
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        // Aktualisiert die Zutat in der JSON-Datei
        private void UpdateZutat(string path, string name)
        {
            EditZutat.KategorieNamen = KategorieList.Split(',').Select(s => s.Trim()).ToList();
            EditZutat.Allergene = AllergenList.Split(',').Select(s => s.Trim()).ToList();

            JsonUtils.UpdateJson<Zutat>(FullPath, name, EditZutat);
            JsonUtils.SortJsonFileZutat(FullPath);

            // Schließe das aktuelle Fenster
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }
    }
}
