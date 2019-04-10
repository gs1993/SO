using System.Windows;
using AdminPanel.ViewModels;

namespace AdminPanel.Views
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
