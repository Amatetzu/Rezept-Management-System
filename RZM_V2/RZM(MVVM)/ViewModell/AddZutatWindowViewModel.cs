using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RZM_MVVM_.Modell;
using RZM_MVVM_.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RZM_MVVM_.ViewModell
{
    internal class AddZutatWindowViewModel : ViewModelBase
    {
        private string oldName { get; set; }
        private Zutat _editZutat;
        public string FullPath = JsonUtils.GetFullPath(ConstValues.ZutatenJsonPath);
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
        public ICommand DeleteZutatCommand => new RelayCommand(ClearDate);

        public AddZutatWindowViewModel()
        {
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

        private void StoraData()
        {
            UpdateZutat(FullPath, EditZutat);
        }

        public Zutat EditZutat
        {
            get { return _editZutat; }
            set { Set(ref _editZutat, value); }
        }

        private void UpdateZutat(string path, Zutat data)
        {
            data.KategorieNamen = KategorieList.Split(',').Select(s => s.Trim()).ToList();
            data.Allergene = AllergenList.Split(',').Select(s => s.Trim()).ToList();

            JsonUtils.WriteJson(path, data);
            JsonUtils.SortJsonFileZutat(path);  // Ensure path is used consistently
            ClearDate();
        }

        private void ClearDate()
        {
            EditZutat = new Zutat();
            AllergenList = "";
            KategorieList = "";
        }
    }
}
