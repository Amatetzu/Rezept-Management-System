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
using RZM_MVVM_.Modell;
using System.Threading.Channels;

namespace RZM_MVVM_.ViewModell
{
    internal class ShowKategorieWindowViewModel : ViewModelBase
    {
        public string FullPath = Path.GetFullPath(ConstValues.KategorienJsonPath);
        private Kategorie _showKategorie;
        public Kategorie ShowKategori { get { return _showKategorie; } set { Set(ref _showKategorie, value); } }
        public string oldName { get; set; }
        public List<Kategorie> kategories { get; set; }
        public ICommand Editkategori => new RelayCommand(EditData);
        private void EditData()
        {
            EditKategorieWidow showWindow = new EditKategorieWidow();
            showWindow.Owner = Application.Current.Windows.OfType<ShowZutatWindow>().FirstOrDefault();
            Messenger.Default.Send(new UpdateKategorieMessage(oldName));
            showWindow.Closed += ShowWindow_Closed;


            showWindow.ShowDialog();
        }


        public ShowKategorieWindowViewModel()
        {
               Messenger.Default.Register<UpdateKategorieMessage>(this, HandleUpdateKategorieMessage);
                 ShowKategori = new Kategorie();
        }

        private void HandleUpdateKategorieMessage(UpdateKategorieMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.NewKategorie))
            {
                MessageBox.Show("Fehler: Ungültige Nachricht erhalten.");
                return;
            }
            ShowKategori.Name = message.NewKategorie;
            oldName = message.NewKategorie;
            UpdateKategie();

        }

        private void UpdateKategie()
        {
            kategories = JsonUtils.GetOneFullData<Kategorie>(FullPath, oldName);
            ShowKategori = kategories[0];
         }

        private event EventHandler Closed;

        private void ShowWindow_Closed(object sender, System.EventArgs e)
        {
          
            try { UpdateKategie(); 
            }
            catch
            {
                Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                currentWindow.Close();
            }
        }
    }
}
