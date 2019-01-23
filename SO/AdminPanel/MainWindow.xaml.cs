using AdminPanel.Utils;
using FirstFloor.ModernUI.Windows.Controls;

namespace AdminPanel
{
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
