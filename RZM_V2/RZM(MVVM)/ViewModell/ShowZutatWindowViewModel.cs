using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using RZM_MVVM_.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class ShowZutatWindowViewModel : ViewModelBase
    {
        // Pfad zur JSON-Datei für Zutaten
        public string FullPath = Path.GetFullPath(ConstValues.ZutatenJsonPath);

        // Das aktuell bearbeitete Zutat-Objekt
        private Zutat _editZutat;
        public Zutat EditZutat
        {
            get { return _editZutat; }
            set { Set(ref _editZutat, value); }
        }

        // Der Name der Zutat, die bearbeitet wird
        private string oldName;

        // Liste der Zutaten
        public List<Zutat> zutats { get; set; }

        // Bindable properties für Allergene und Kategorien
        private string _allergenList;
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

        private string _kategorieList;
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

        private string _energie;
        public string Energie
        {
            get { return _energie; }
            set { Set(ref _energie, value); }
        }

        // Command zum Bearbeiten der Zutat
        public ICommand EditZutatCommand => new RelayCommand(EditData);

        // Konstruktor
        public ShowZutatWindowViewModel()
        {
            Messenger.Default.Register<UpdateZutatMessage>(this, HandleUpdateZutatMessage);
            EditZutat = new Zutat();
        }

        // Handler für die Update-Zutat-Nachricht
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

        // Methode zum Öffnen des Bearbeitungsfensters
        private void EditData()
        {
            var editZutatWindow = new EditZutatWindow
            {
                Owner = Application.Current.Windows.OfType<ShowZutatWindow>().FirstOrDefault()
            };

            Messenger.Default.Send(new UpdateZutatMessage(EditZutat.Name));
            editZutatWindow.Closed += ShowWindow_Closed;
            editZutatWindow.ShowDialog();
        }

        // Methode zum Aktualisieren der Zutat
        private void UpdateList()
        {
            zutats = JsonUtils.GetOneFullData<Zutat>(FullPath, oldName);
            if (zutats == null || zutats.Count == 0)
            {
                MessageBox.Show("Fehler: Zutat konnte nicht geladen werden.");
                return;
            }

            EditZutat = zutats[0];
            AllergenList = string.Join(", ", EditZutat.Allergene);
            KategorieList = string.Join(", ", EditZutat.KategorieNamen);
            Energie = $"{EditZutat.EnergieKcal} kcal";
        }

        // Event-Handler für das Schließen des Bearbeitungsfensters
        private void ShowWindow_Closed(object sender, System.EventArgs e)
        {
            try
            {
                UpdateList();
            }
            catch
            {
                var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                currentWindow?.Close();
            }
        }

        // Aufräumen der Ressourcen
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }
    }
}
