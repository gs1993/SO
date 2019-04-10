using AdminPanel.Api;
using AdminPanel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Pages.Users
{
    public class UserDetailViewModel : ViewModelBase, IUserDetailViewModel
    {
        private UserDetailsDto _user;


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
    }
}
