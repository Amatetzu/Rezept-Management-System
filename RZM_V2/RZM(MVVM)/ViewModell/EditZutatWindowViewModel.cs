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
        private string oldName { get; set; }
        private Zutat _editZutat;
        public string FullPath = Path.GetFullPath(ConstValues.ZutatenJsonPath);
        public List<Zutat> zutats { get; set; }

        private string _allergenList;
        private string _kategorieList;

        public string AllergenList
        {
            get { return _allergenList; }
            set
            {
                Set(ref _allergenList, value);
                EditZutat.Allergene = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }

        public string KategorieList
        {
            get { return _kategorieList; }
            set
            {
                Set(ref _kategorieList, value);
                EditZutat.KategorieNamen = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }

        public ICommand UpdateZutatCommand => new RelayCommand(StoraData);
        public ICommand DeleteZutatCommand => new RelayCommand(DeleteZutat);

        public EditZutatWindowViewModel()
        {
            Messenger.Default.Register<UpdateZutatMessage>(this, HandleUpdateZutatMessage);
            EditZutat = new Zutat();
        }

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

        private void StoraData()
        {
            UpdateZutat(FullPath, oldName);
        }

        private void DeleteZutat() {
            MessageBoxResult result = MessageBox.Show("Wollen SIe die Zutat wirklich Löschen?", "Löschen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes )
            {
                JsonUtils.DeleteJson<Zutat>(FullPath, oldName);
                Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }
            }
                }

        private void UpdateList()
        {
            zutats = JsonUtils.GetOneFullData<Zutat>(FullPath, oldName);
            if (zutats == null)
            {
                MessageBox.Show("Fehler: Zutat konnte nicht geladen werden.");
                return;
            }
            EditZutat = zutats[0];
            AllergenList = string.Join(", ", EditZutat.Allergene);
            KategorieList = string.Join(", ", EditZutat.KategorieNamen);
        }

        public Zutat EditZutat
        {
            get { return _editZutat; }
            set { Set(ref _editZutat, value); }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        private void UpdateZutat(string path, string name)
        {
            EditZutat.KategorieNamen = KategorieList.Split(',').ToList();
            EditZutat.Allergene = AllergenList.Split(',').ToList();
            
            JsonUtils.UpdateJson<Zutat>(FullPath, name, EditZutat);
            JsonUtils.SortJsonFileZutat(FullPath);
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

    }
}
