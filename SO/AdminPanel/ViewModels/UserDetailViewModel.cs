using AdminPanel.Api;
using AdminPanel.Events;
using AdminPanel.Utils;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
    {
        private UserDetailsDto _user;
        private IEventAggregator _eventAggregator;

        public UserDetailViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenUserDetailsEvent>()
                .Subscribe(OnOpenUserDetailView);
        }
        
        public UserDetailsDto User
        {
            get { return _user; }
            set
            {
                _user = value;
                Notify();
            }
        }


        public async Task LoadAsync(int id)
        {
            var result = await ApiClient.GetUserDetails(id);
            if (result.IsFailure) { }

            User = result.Value;
        }


        private async void OnOpenUserDetailView(int id)
        {
            await LoadAsync(id);
        }
    }
}
