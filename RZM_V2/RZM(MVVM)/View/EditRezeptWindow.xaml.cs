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
    /// Interaktionslogik für EditRezeptWindow.xaml
    /// </summary>
    public partial class EditRezeptWindow : Window
    {
        public EditRezeptWindow()
        {
            InitializeComponent();
            EditRezeptWindowViewModel viewModel = new EditRezeptWindowViewModel();
            DataContext = viewModel;
        }

        private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as EditRezeptWindowViewModel;
            if (viewModel != null && viewModel.ItemClickCommand.CanExecute(null))
            {
                viewModel.ItemClickCommand.Execute(null);
            }
        }
    }
}
