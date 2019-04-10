using AdminPanel.Api;
using AdminPanel.Utils;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class UserNavigationViewModel : ViewModelBase, IUserNavigationViewModel
    {
        private ObservableCollection<LastUserDto> _users;

        public UserNavigationViewModel()
        {
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


        public async Task LoadAsync()
        {
            var result = await ApiClient.GetLastCreatedUsersAsync();
            if (result.IsFailure)
            {
                // TODO: message
            }

            Users = new ObservableCollection<LastUserDto>(result.Value);
        }
    }
}
