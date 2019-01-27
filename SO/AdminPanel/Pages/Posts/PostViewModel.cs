using AdminPanel.Api;
using AdminPanel.Utils;

namespace AdminPanel.Pages.Posts
{
    public class PostViewModel : ViewModelBase
    {
        private PostDetailsDto _post;

        public PostDetailsDto Post
        {
            get { return _post; }
            set
            {
                _post = value;
                OnPropertyChanged("Post");
            }
        }

        public PostViewModel(PostDetailsDto post = null)
        {
            Post = post;
        }

    }
}
