using System;
using System.Windows;
using AdminPanel.Api;
using AdminPanel.Startup;
using Autofac;

namespace AdminPanel
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApiClient.Init("http://localhost:5100/api");

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender,
          System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Unexpected error occured. Please inform the admin. {Environment.NewLine}  {e.Exception.Message}, Unexpected error");

            e.Handled = true;
        }
    }
}
