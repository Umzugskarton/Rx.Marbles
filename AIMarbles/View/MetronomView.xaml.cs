using AIMarbles.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIMarbles.View
{
    /// <summary>
    /// Interaction logic for MetronomView.xaml
    /// </summary>
    public partial class MetronomView : UserControl
    {

        public MetronomView()
        {
            InitializeComponent();

            this.Unloaded += MetronomView_Unloaded;
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void MetronomView_Unloaded(object sender, RoutedEventArgs e)
        {
            // Dispose of the ViewModel when the View is unloaded
            // This is crucial for cleaning up Rx.NET subscriptions and preventing memory leaks.
            if (DataContext is IDisposable disposableViewModel)
            {
                disposableViewModel.Dispose();
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, "[0-9]");
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (sender is not TextBox textBox) { return; }

            string currentText = textBox.Text;
            string newText = Regex.Replace(currentText,  "[^0-9]", "");

            if (newText == currentText) { return; }

            // Should already be handled by MaxLength="3" in the XAML
            if (newText.Length > 3) { newText = newText.Substring(0, 3); }

            int caretIndex = textBox.CaretIndex;
            textBox.Text = newText;
            textBox.CaretIndex = Math.Min(caretIndex, newText.Length);
        }

    }
}
