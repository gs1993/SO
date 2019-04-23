using AdminPanel.Events;
using AdminPanel.Utils;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IUserDetailViewModel _userDetailViewModel;


        public MainWindowViewModel(IUserNavigationViewModel userNavigationViewModel, IUserDetailViewModel userDetailViewModel,
            IEventAggregator eventAggregator)
        {
            UserNavigationViewModel = userNavigationViewModel;
            UserDetailViewModel = userDetailViewModel;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterUserBannedEvent>().Subscribe(RemoveUserDetailAfterBan);
        }

        
        public IUserNavigationViewModel UserNavigationViewModel { get; }
        public IUserDetailViewModel UserDetailViewModel {
            get
            {
                return _userDetailViewModel;
            }
            private set
            {
                _userDetailViewModel = value;
                Notify();
            }
        }


        public async Task Load()
        {
            await UserNavigationViewModel.LoadAsync();
        }

        private void RemoveUserDetailAfterBan(int bannedUserId)
        {
            UserDetailViewModel = null;
        }
    }
}
