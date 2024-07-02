using System;
using System.Windows;
using System.Windows.Controls;

namespace Rezept_Managment_System.userControlls
{
    public partial class GenericButton : UserControl
    {
        public GenericButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(GenericButton), new PropertyMetadata(null));

        public new object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public event EventHandler GenericButtonClicked;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            GenericButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
