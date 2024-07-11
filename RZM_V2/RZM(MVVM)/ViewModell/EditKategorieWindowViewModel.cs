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
    class EditKategorieWindowViewModel : ViewModelBase
    {
        public string FullPath = Path.GetFullPath(ConstValues.KategorienJsonPath);
        private Kategorie _showKategorie;
        public Kategorie ShowKategori { get { return _showKategorie; } set { Set(ref _showKategorie, value); } }
        public string oldName { get; set; }
        public List<Kategorie> kategories { get; set; }
        public ICommand StoraKategorie => new RelayCommand(storaKategorie);
        private void storaKategorie()
        {
            JsonUtils.UpdateJson<Kategorie>(FullPath,oldName,ShowKategori);
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow.Close();
        }

        public ICommand DeleteKategorie => new RelayCommand(DeleteData);
        private void DeleteData()
        {
            JsonUtils.DeleteJson<Kategorie>(FullPath, oldName);
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow.Close();
        }
        public EditKategorieWindowViewModel()
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
    }
}
