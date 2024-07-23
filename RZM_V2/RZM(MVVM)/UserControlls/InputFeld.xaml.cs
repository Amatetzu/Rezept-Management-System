using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RZM_MVVM_.MVVM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RZM_MVVM_.Modell;

namespace RZM_MVVM_.UserControlls
{
    public partial class InputFeld : UserControl
    {
        public InputFeld()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InputFeld), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(InputFeld), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public ICommand ClearCommand
        {
            get { return new RelayCommand(ClearText); }
        }

        private void ClearText()
        {
            Text = string.Empty;
        }


    }
}
