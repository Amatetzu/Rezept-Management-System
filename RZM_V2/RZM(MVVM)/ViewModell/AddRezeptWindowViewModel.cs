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
        List<Rezept> rezept { get; set; }

        public AddRezeptWindowViewModel(){
            RezeptName = "Neues Rezept";
            Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);
            RezeptZubereitung = "Hier steht die Zubereitung";
            



        }

        private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
        {
            RezeptName = message.NewHeader;
            UpdateRezept();
            
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        public void UpdateRezept()
        {
            rezept = JsonUtils.GetOneFullData<Rezept>(FullPath, RezeptName);
            
            RezeptZubereitung = rezept[0].Zubereitung;
            
        }



    }
}
