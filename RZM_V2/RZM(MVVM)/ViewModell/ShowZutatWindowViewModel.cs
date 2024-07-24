using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using RZM_MVVM_.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class ShowZutatWindowViewModel : ViewModelBase
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

        private string _energie;
        public string Energie
        {
            get { return _energie; }
            set { Set(ref _energie, value); }
        }

        public ICommand EditZutatCommand => new RelayCommand(EditData);

        public ShowZutatWindowViewModel()
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
        private event EventHandler Closed;
        private void ShowWindow_Closed(object sender, System.EventArgs e)
        {
            try { UpdateList(); }
            catch {
                Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                currentWindow.Close();
            }
        }

        

        private void EditData()
        {
            EditZutatWindow showWindow = new EditZutatWindow();
            showWindow.Owner = Application.Current.Windows.OfType<ShowZutatWindow>().FirstOrDefault();
            Messenger.Default.Send(new UpdateZutatMessage(EditZutat.Name));
            showWindow.Closed += ShowWindow_Closed;


            showWindow.ShowDialog();
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
            Energie = EditZutat.EnergieKcal.ToString() + " kcal";
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
        

    }
}
