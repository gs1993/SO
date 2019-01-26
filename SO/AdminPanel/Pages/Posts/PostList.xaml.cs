using System.Threading;
using System.Windows.Controls;

namespace AdminPanel.Pages.Posts
{
    public partial class PostList : UserControl
    {
        public PostList()
        {
            InitializeComponent();

            DataContext = new PostListViewModel();
        }
    }
}
