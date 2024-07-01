using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rezept_Managment_System.userControlls
{
    public partial class UserInputLable : UserControl, INotifyPropertyChanged
    {
        private string _userInput;

        public string UserInput
        {
            get { return _userInput; }
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged("UserInput");
                }
            }
        }
        public UserInputLable()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _inputName;

        public string InputName
        {
            get { return _inputName; }
            set
            {
                if (_inputName != value)
                {
                    _inputName = value;
                    OnPropertyChanged("InputName");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput = string.Empty;
            
            input.Focus();
            
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholder.Visibility = string.IsNullOrEmpty(UserInput) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
