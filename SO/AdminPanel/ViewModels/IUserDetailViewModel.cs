using System.Threading.Tasks;
using AdminPanel.Api;

namespace AdminPanel.ViewModels
{
    public interface IUserDetailViewModel
    {
        UserDetailsDto User { get; set; }

        Task LoadAsync(int id);
    }
}