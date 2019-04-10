using System.Threading.Tasks;
using AdminPanel.Api;

namespace AdminPanel.Pages.Users
{
    public interface IUserDetailViewModel
    {
        UserDetailsDto User { get; set; }

        Task LoadAsync(int id);
    }
}