using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace RZM_MVVM_.Behaviors
{
    
    
    public static class TextChangedBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(TextChangedBehavior), new PropertyMetadata(OnCommandChanged));

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.OldValue != null)
                {
                    textBox.TextChanged -= OnTextChanged;
                }

                if (e.NewValue != null)
                {
                    textBox.TextChanged += OnTextChanged;
                }
            }
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var command = GetCommand(textBox);

            if (command != null && command.CanExecute(textBox.Text))
            {
                command.Execute(textBox.Text);
            }
        }
    }
}
