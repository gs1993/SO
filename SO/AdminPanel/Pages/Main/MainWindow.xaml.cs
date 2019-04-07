using System.Windows;
using AdminPanel.Pages.Main;
using FirstFloor.ModernUI.Windows.Controls;

namespace AdminPanel
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            _viewModel = mainWindowViewModel;
            DataContext = _viewModel;
            Loaded += MainWindowLoaded;

            InitializeComponent();
        }

        private async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.Load();
        }
    }
}
