using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls;

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
