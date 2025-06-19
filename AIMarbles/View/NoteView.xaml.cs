using AIMarbles.MusicTheory;
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
    /// Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView : UserControl
    {
        private static readonly Regex _noteRegex = new Regex(
            @"^(?:C(?:#)?|D(?:#|b)?|E(?:b)?|F(?:#)?|G(?:#|b)?|A(?:#|b)?|B(?:b)?)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled 
        );

        private static NoteName currentNote;
        public NoteView()
        {
            InitializeComponent();
        }
        private void MusicNoteTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox textBox) { return; }
            e.Handled = !IsTextAllowed(String.Concat(textBox.Text, e.Text));
        }

        private static bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text,_noteRegex.ToString());
        }

        private void MusicNoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (sender is not TextBox textBox) { return; }

            string currentText = textBox.Text;
            string newText = Regex.Replace(currentText, _noteRegex.ToString(), "");

            if (newText == currentText) { return; }

            // Should already be handled by MaxLength="3" in the XAML
            if (newText.Length > 3) { newText = newText.Substring(0, 3); }

            int caretIndex = textBox.CaretIndex;
            textBox.Text = newText;
            textBox.CaretIndex = Math.Min(caretIndex, newText.Length);
        }
    }
}
