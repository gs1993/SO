using AdminPanel.Api;
using AdminPanel.Pages.Users;
using AdminPanel.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AdminPanel.Pages.Main
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
