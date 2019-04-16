using AdminPanel.Api;
using AdminPanel.Events;
using AdminPanel.Utils;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class UserNavigationViewModel : ViewModelBase, IUserNavigationViewModel
    {
        private ObservableCollection<LastUserDto> _users;
        private LastUserDto _selectedUserDto;
        private IEventAggregator _eventAggregator;

        public UserNavigationViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterUserBannedEvent>().Subscribe(AfterUserBan);

            _users = new ObservableCollection<LastUserDto>();
        }

        
        public ObservableCollection<LastUserDto> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                Notify();
            }
        }

        public LastUserDto SelectedUserDto
        {
            get { return _selectedUserDto; }
            set {
                _selectedUserDto = value;
                Notify();
                if (_selectedUserDto != null)
                    _eventAggregator.GetEvent<OpenUserDetailsEvent>()
                        .Publish(_selectedUserDto.Id);
            }
        }


        public async Task LoadAsync()
        {
            var result = await ApiClient.GetLastCreatedUsersAsync(25);
            if (result.IsFailure)
            {
                // TODO: message
            }

            Users = new ObservableCollection<LastUserDto>(result.Value);
        }

        private void AfterUserBan(int deletedUserId)
        {
            var userToRemove = Users.FirstOrDefault(u => u.Id == deletedUserId);
            if (userToRemove != null)
                Users.Remove(userToRemove);

            // TODO: Load next user on the bottom of the table
        }
    }
}
