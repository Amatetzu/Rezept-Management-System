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
    class AddKategorieWindowViewModel:ViewModelBase
        {
            public string FullPath = JsonUtils.GetFullPath(ConstValues.KategorienJsonPath);
            private Kategorie _showKategorie;
            public Kategorie ShowKategori { get { return _showKategorie; } set { Set(ref _showKategorie, value); } }
            public ICommand StoraKategorie => new RelayCommand(storaKategorie);
            private void storaKategorie()
            {
            JsonUtils.WriteJson(FullPath, ShowKategori);
            DeleteData();
            JsonUtils.SortJsonFileKategorie(FullPath);
            }

            public ICommand DeleteKategorie => new RelayCommand(DeleteData);
            private void DeleteData()
            {
            ShowKategori = new Kategorie();
            }
            public AddKategorieWindowViewModel()
            {
             
                ShowKategori = new Kategorie();
            }
            
            
        
    }
}
