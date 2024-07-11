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
    /// Interaktionslogik für AddKategorieWindow.xaml
    /// </summary>
    public partial class AddKategorieWindow : Window
    {
        public AddKategorieWindow()
        {
            InitializeComponent();
            AddKategorieWindowViewModel vm = new AddKategorieWindowViewModel();
            this.DataContext = vm;
        }
    }
}
