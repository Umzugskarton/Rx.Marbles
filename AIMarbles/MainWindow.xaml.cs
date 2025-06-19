using AIMarbles.Core.Service;
using AIMarbles.ViewModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIMarbles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is not MainWindowViewModel viewModel)
            {
                return;
            }
            if (!viewModel.CancelConnectionModeCommand.CanExecute(null)) { return;  }
            // Execute the command
            viewModel.CancelConnectionModeCommand.Execute(null);
        }

    }
}