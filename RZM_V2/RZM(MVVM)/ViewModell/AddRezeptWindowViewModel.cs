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

        private Rezept _newRezept;
        public Rezept NewRezept
        {
            get { return _newRezept; }
            set { Set(ref _newRezept, value); }
        }

        public AddRezeptWindowViewModel() {
        
        }


    }
}
