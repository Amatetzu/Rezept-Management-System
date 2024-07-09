using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class EditZutatWindowViewModel : ViewModelBase
    {
        private string oldName { get; set; }
        private Zutat _editZutat;
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.ZutatenJsonPath);
        public List<Zutat> zutats { get; set; }

        public EditZutatWindowViewModel()
        {
            // Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);
            Messenger.Default.Register<UpdateZutatMessage>(this, HandleUpdateZutatMessage);

            // Initialisiere EditZutat
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
        }

        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            if (EditZutat == null)
            {
                MessageBox.Show("Fehler: EditZutat ist null.");
                return;
            }

            EditZutat.Name = "test";
            oldName = EditZutat.Name;
        }

        private void UpdateRezept()
        {
            zutats = JsonUtils.GetOneFullData<Zutat>(FullPath, oldName);
            if (zutats == null)
            {
                MessageBox.Show("Fehler: Zutat konnte nicht geladen werden.");
                return;
            }
            EditZutat= zutats[0];
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
