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
    internal class ShowRezeptWindwoViewModell : ViewModelBase
    {
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        public ICommand TestCommand => new RelayCommand(TestFunktion);
        public ICommand EditCommand => new RelayCommand(EditRezept);
        public string RezeptName { get; set; }

        private Rezept _showrezept;
        public Rezept ShowRezept
        {
            get { return _showrezept; }
            set
            {
                _showrezept = value;
                RaisePropertyChanged();
            }
        }

        private string _showRezeptZutaten;
        public string ShowRezeptZutaten
        {
            get { return _showRezeptZutaten; }
            set
            {
                _showRezeptZutaten = value;
                RaisePropertyChanged();
            }
        }
        private string _showRezeptKategorien;
        public string ShowRezeptKategorien
        {
            get { return _showRezeptKategorien; }
            set
            {
                _showRezeptKategorien = value;
                RaisePropertyChanged();
            }
        }
        private string _showRezeptAllergene;
        public string ShowRezeptAllergene
        {
            get { return _showRezeptAllergene; }
            set
            {
                _showRezeptAllergene = value;
                RaisePropertyChanged();
            }
        }

        public List<Rezept> rezept { get; set; }

        public ShowRezeptWindwoViewModell()
        {
            Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);

            Application.Current.MainWindow.Closed += (s, e) => Application.Current.Shutdown();
        }

        public void TestFunktion()
        {
            MessageBox.Show("Test");
        }

        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            RezeptName = message.NewHeader;
            UpdateRezeptZubereitung();
        }

        private void UpdateRezeptZubereitung()
        {
            if (string.IsNullOrEmpty(RezeptName))
            {
                MessageBox.Show("RezeptName is not set.");
                return;
            }

            rezept = JsonUtils.GetOneFullData<Rezept>(FullPath, RezeptName);

            if (rezept != null && rezept.Count > 0)
            {
                ShowRezept = rezept[0];
            }
            else
            {
                MessageBox.Show("No recipe found with the given name.");
            }
            for (int i = 0; i < ShowRezept.Zutaten.Count; i++)
            {
                ShowRezeptZutaten += ShowRezept.Zutaten[i].Name + " " + ShowRezept.Zutaten[i].Menge + "; " ;
            }
            for (int i = 0; i < ShowRezept.Kategorien.Count; i++)
            {
                ShowRezeptKategorien += ShowRezept.Kategorien[i] + "; ";
            }
            for (int i = 0; i < ShowRezept.Allergene.Count; i++)
            {
                ShowRezeptAllergene += ShowRezept.Allergene[i] + "; ";
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        public void EditRezept()
        {
            View.EditRezeptWindow editRezeptWindow = new View.EditRezeptWindow();
            editRezeptWindow.Owner = Application.Current.Windows.OfType<View.ShowRezeptWindow>().FirstOrDefault();
            Messenger.Default.Send(new UpdateHeaderMessage(ShowRezept.Name));
            editRezeptWindow.ShowDialog();
        }
    }
}
