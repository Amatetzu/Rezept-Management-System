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
    internal class AddRezeptWindowViewModel : ViewModelBase
    {
        public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

        private string _rezeptName;
        public string RezeptName
        {
            get { return _rezeptName; }
            set { Set(ref _rezeptName, value); }
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

        public AddRezeptWindowViewModel(){
            RezeptName = "Neues Rezept";
        }
    }
}
