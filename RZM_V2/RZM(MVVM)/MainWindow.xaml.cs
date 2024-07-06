using Microsoft.Xaml.Behaviors.Layout;
using RZM_MVVM_.ViewModell;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RZM_MVVM_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event MouseButtonEventHandler ListDoubleClick;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;

            // Event Handler für das MouseDoubleClick-Ereignis der gnereicList
            gnereicList.MouseDoubleClick += gnereicList_MouseDoubleClick;
        }

        private void gnereicList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ereignis ListDoubleClick auslösen
            ListDoubleClick?.Invoke(sender, e);
        }
    }
}
