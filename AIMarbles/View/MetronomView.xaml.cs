using AIMarbles.ViewModel;
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
    /// Interaction logic for MetronomView.xaml
    /// </summary>
    public partial class MetronomView : UserControl
    {
        public MetronomView()
        {
            InitializeComponent();
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, "[0-9]");
        }

        private void MetronomView_Loaded(object sender, RoutedEventArgs e)
        {
            passDimensionsToVM();
        }

        private void MetronomView_SizeChanged(object sender, RoutedEventArgs e)
        {
            passDimensionsToVM();
        }

        private void passDimensionsToVM()
        {
            if (this.DataContext is not MetronomViewModel viewModel) { return; }
            double viewWidth = this.ActualWidth;
            double viewHeight = this.ActualHeight;

            viewModel.UpdateViewDimensions(viewWidth, viewHeight);

        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            TextBox? textBox = sender as TextBox;

            if (textBox == null) {
                return;
            }

            string currentText = textBox.Text;
            string newText = Regex.Replace(currentText, "[^0-9]", "");

            if (newText.Length > 3)
            {
                newText = newText.Substring(0, 3);
            }

            if (newText == currentText)
            {
                return;
            }

            int caretIndex = textBox.CaretIndex;
            textBox.Text = newText;
            textBox.CaretIndex = Math.Min(caretIndex, newText.Length);

        }
    }
}
