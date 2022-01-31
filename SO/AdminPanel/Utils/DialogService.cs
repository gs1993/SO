using System.Windows;

namespace AdminPanel.Utils
{
    public class DialogService
    {
        public bool? ShowDialog(ViewModelBase viewModel)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Window owner = System.Windows.Application.Current.MainWindow;

                var window = new CustomWindow(viewModel)
                {
                    ShowInTaskbar = false,
                    Owner = Application.Current.MainWindow
                };
                window.ShowDialog();
            });

            return true;
        }
    }
}
