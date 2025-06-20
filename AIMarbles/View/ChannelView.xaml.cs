using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIMarbles.View
{
    /// <summary>
    /// Interaction logic for ChannelView.xaml
    /// </summary>
    public partial class ChannelView : UserControl
    {
        public ChannelView()
        {
            InitializeComponent();

            this.Unloaded += ChannelView_Unloaded;
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, "[0-9]");
        }

        private void ChannelView_Unloaded(object sender, RoutedEventArgs e)
        {
            // Dispose of the ViewModel when the View is unloaded
            // This is crucial for cleaning up Rx.NET subscriptions and preventing memory leaks.
            if (DataContext is IDisposable disposableViewModel)
            {
                disposableViewModel.Dispose();
            }
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (sender is not TextBox textBox) { return; }

            string currentText = textBox.Text;
            string newText = Regex.Replace(currentText,  "[^0-9]", "");

            if (newText == currentText) { return; }

            // Should already be handled by MaxLength="3" in the XAML
            if (newText.Length > 1) { newText = newText.Substring(0, 1); }

            int caretIndex = textBox.CaretIndex;
            textBox.Text = newText;
            textBox.CaretIndex = Math.Min(caretIndex, newText.Length);
        }
    }
}
