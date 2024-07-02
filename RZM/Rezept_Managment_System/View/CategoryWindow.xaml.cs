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

namespace Rezept_Managment_System.View
{
    /// <summary>
    /// Interaktionslogik für CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
        }

        

        private void edit_GenericButtonClicked(object sender, EventArgs e)
        {
            
        }

        private void add_GenericButtonClicked(object sender, EventArgs e)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.ShowDialog();
        }

        private void delete_GenericButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
