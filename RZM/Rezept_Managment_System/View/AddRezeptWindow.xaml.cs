using Rezept_Managment_System.utilities;
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
using Rezept_Managment_System.utilities;
using Rezept_Managment_System.userControlls;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Rezept_Managment_System.View
{
   
    public partial class AddRezeptWindow : Window
    {
        public AddRezeptWindow()
        {
            InitializeComponent();
        }
        Rezept rezept = new Rezept();
        List<Rezept> rezepts = new List<Rezept>();

        public void saveRezept()
        {
            rezept.Name = name.UserInput;
            rezept.Zubereitung = description.UserInput;
            rezept.Zeit = Int32.Parse(time.UserInput);
            rezept.Kategorien = ManageJson.SplitString(category.UserInput);

            rezept.Allergene = ManageJson.SplitString(allerg.UserInput);

            rezepts.Add(rezept);

            ManageJson.WriteJsonFile(ManageJson.RezeptePath, rezepts);



        }
        private void add_GenericButtonClicked(object sender, EventArgs e)
        {
            try {
                saveRezept();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void ingredients_AddButtonClicked(object sender, EventArgs e)
        {
            rezept.Zutaten.Add(ingredients.UserInputIng + ", " + ingredients.UserInputAmount);

        }
    }
}
