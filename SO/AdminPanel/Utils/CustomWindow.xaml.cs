namespace AdminPanel.Utils
{
    public partial class CustomWindow
    {
        public CustomWindow(ViewModelBase viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            //Width = viewModel.Width;
            //Height = viewModel.Height;
        }
    }
}
