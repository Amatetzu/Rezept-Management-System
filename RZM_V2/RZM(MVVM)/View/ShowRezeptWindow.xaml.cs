﻿using RZM_MVVM_.ViewModell;
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
    /// Interaktionslogik für ShowRezeptWindow.xaml
    /// </summary>
    public partial class ShowRezeptWindow : Window
    {
        public ShowRezeptWindow()
        {
            InitializeComponent();
            ShowRezeptWindwoViewModell vm = new ShowRezeptWindwoViewModell();
            DataContext = vm;
        }
    }
}
