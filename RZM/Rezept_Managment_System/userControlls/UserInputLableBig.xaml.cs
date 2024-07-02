using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Rezept_Managment_System.userControlls
{
    /// <summary>
    /// Interaktionslogik für UserInputLableBig.xaml
    /// </summary>
    public partial class UserInputLableBig : UserControl, INotifyPropertyChanged
    {
        private string _userInput;
        private string _inputName;

        public UserInputLableBig()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string UserInput
        {
            get { return _userInput; }
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged(nameof(UserInput));
                }
            }
        }

        public string InputName
        {
            get { return _inputName; }
            set
            {
                if (_inputName != value)
                {
                    _inputName = value;
                    OnPropertyChanged(nameof(InputName));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
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
