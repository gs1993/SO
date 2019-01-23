using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Pages.Posts;

namespace AdminPanel.Utils
{
    public class MainViewModel
    {
        public ViewModelBase ViewModel { get; }

        public MainViewModel()
        {
           ViewModel = new PostListViewModel();
        }
    }
}
