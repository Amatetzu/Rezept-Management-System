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

namespace RZM_MVVM_.View
{
    /// <summary>
    /// Interaktionslogik für EditKategorieWidow.xaml
    /// </summary>
    public partial class EditKategorieWidow : Window
    {
        public EditKategorieWidow()
        {
            InitializeComponent();
            EditKategorieWindowViewModel vm = new EditKategorieWindowViewModel();
            DataContext = vm;
        }
    }
}
