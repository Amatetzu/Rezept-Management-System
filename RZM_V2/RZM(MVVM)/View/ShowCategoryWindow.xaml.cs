using RZM_MVVM_.ViewModell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RZM_MVVM_.ViewModell;


namespace RZM_MVVM_.View
{
    /// <summary>
    /// Interaktionslogik für ShowCategoryWindow.xaml
    /// </summary>
    public partial class ShowCategoryWindow : Window
    {
        public ShowCategoryWindow()
        {
            InitializeComponent();
            ShowCategoryWindowViewModel vm = new ShowCategoryWindowViewModel();
            DataContext = vm;
        }
    }
}
