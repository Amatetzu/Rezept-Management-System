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
    internal class ShowRezeptWindwoViewModell : ViewModelBase
    {
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        public ICommand TestCommand => new RelayCommand(TestFunktion);
        public ICommand EditCommand => new RelayCommand(EditRezept);

        private string _headerRezept;
        public string HeaderRezept
        {
            get { return _headerRezept; }
            set { Set(ref _headerRezept, value); }
        }

        private string _rezeptZutaten;
        public string RezeptZutaten
        {
            get { return _rezeptZutaten; }
            set { Set(ref _rezeptZutaten, value); }
        }

        private string _rezeptZubereitung;
        public string RezeptZubereitung
        {
            get { return _rezeptZubereitung; }
            set { Set(ref _rezeptZubereitung, value); }
        }

        public List<Rezept> rezept { get; set; }

        public ShowRezeptWindwoViewModell()
        {
            // Initialisiere die ObservableCollection
            HeaderRezept = "Spaghetti Bolognese";

            Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);

            // Initial Rezept laden
            UpdateRezeptZubereitung();
            Application.Current.MainWindow.Closed += (s, e) => Application.Current.Shutdown();
        }

        public void TestFunktion()
        {
            MessageBox.Show("Test");
        }

        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            HeaderRezept = message.NewHeader;
            UpdateRezeptZubereitung();
        }

        private void UpdateRezeptZubereitung()
        {
            rezept = JsonUtils.GetOneFullData<Rezept>(FullPath, HeaderRezept);
            if (rezept.Count > 0)
            {
                RezeptZubereitung = rezept[0].Zubereitung;
            }
            else
            {
                RezeptZubereitung = ""; // Setze auf einen Standardwert oder handle leere Ergebnisse
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
            Messenger.Default.Send(new UpdateHeaderMessage(HeaderRezept));
            editRezeptWindow.ShowDialog();
        }
    }
}
