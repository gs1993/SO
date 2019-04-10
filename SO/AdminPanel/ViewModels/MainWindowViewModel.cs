using AdminPanel.Utils;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IUserNavigationViewModel userNavigationViewModel, IUserDetailViewModel userDetailViewModel)
        {
            UserNavigationViewModel = userNavigationViewModel;
            UserDetailViewModel = userDetailViewModel;
        }


        public IUserNavigationViewModel UserNavigationViewModel { get; }
        public IUserDetailViewModel UserDetailViewModel { get; }


        public async Task Load()
        {
            await UserNavigationViewModel.LoadAsync();
        }
    }
}
