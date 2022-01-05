using AdminPanel.Api;
using AdminPanel.Events;
using AdminPanel.Utils;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminPanel.ViewModels
{
    public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
    {
        private UserDetailsDto _user;
        private IEventAggregator _eventAggregator;
        private IMessageService _messageService;

        public UserDetailViewModel(IEventAggregator eventAggregator, IMessageService messageService)
        {
            _eventAggregator = eventAggregator;
            _messageService = messageService;
            _eventAggregator.GetEvent<OpenUserDetailsEvent>()
                .Subscribe(OnOpenUserDetailView);

            BanUserCommand = new DelegateCommand(OnBanExecute);
        }

        public ICommand BanUserCommand { get; }

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

        private async void OnBanExecute()
        {
            await ApiClient.BanUser(User.Id);

            _eventAggregator.GetEvent<AfterUserBannedEvent>().Publish(User.Id);
        }
    }
}
