using System.Windows;
using AdminPanel.Api;

namespace AdminPanel
{
    public partial class App : Application
    {
        public App()
        {
            ApiClient.Init("http://localhost:5100/api");
        }
    }
}
