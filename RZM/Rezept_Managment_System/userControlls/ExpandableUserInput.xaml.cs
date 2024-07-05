using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Rezept_Managment_System.userControlls
{


    public partial class ExpandableUserInput : UserControl , INotifyPropertyChanged

    {
        private string userInputIng;
        private string userInputAmount;

        public string UserInputIng
        {
            get { return userInputIng; }
            set
            {
                if (userInputIng != value)
                {
                    userInputIng = value;
                    OnPropertyChanged("UserInputIng");
                }
            }
        }

        public string UserInputAmount
            {
            get { return userInputAmount; }
            set
            {
                if (userInputAmount != value)
                {
                    userInputAmount = value;
                    OnPropertyChanged("UserInputAmount");
                }
            }
        }
        public ExpandableUserInput()
        {
            InitializeComponent();
            InputList = new ObservableCollection<string>();
            DataContext = this;
        }

        private ObservableCollection<string> inputList;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler AddButtonClicked;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> InputList
        {
            get { return inputList; }
            set { inputList = value; }
        }

        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            expandpopup.IsOpen = !expandpopup.IsOpen;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddButtonClicked?.Invoke(this, EventArgs.Empty);
            string zutaten = UserInputIng + ", " + UserInputAmount;
            
            InputList.Add(zutaten);
            ingInput.Clear();
            amountInput.Clear();
            ingInput.Focus();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            expandpopup.IsOpen = false;
        }
    }
}
