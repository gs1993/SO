using System.Collections.Generic;
using System.Collections.ObjectModel;
using AdminPanel.Api;
using AdminPanel.Utils;

namespace AdminPanel.Pages.Posts
{
    public class PostListViewModel : ViewModelBase
    {
        public PostListViewModel()
        {
            PageSize = 25;

            DeletePostCommand = new Command<PostListDto>(x => x != null, DeletePost);
            UpdatePostCommand = new Command<PostListDto>(x => x != null, UpdatePost);
            
            GetPostPage(1, PageSize);
        }

        public Command<PostListDto> DeletePostCommand { get; }
        public Command<PostListDto> UpdatePostCommand { get; }


        private IReadOnlyList<PostListDto> _postList;
        private int _pageSize;

        public IReadOnlyList<PostListDto> PostList
        {
            get => _postList;
            set
            {
                _postList = value;
                OnPropertyChanged("PostList");
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        public async void GetPostPage(int pageNumber, int pageSize)
        {
            PostList = await ApiClient.GetPostPage(1, 25) ?? new List<PostListDto>();

            Notify(nameof(PostList));
        }

        private async void UpdatePost(PostListDto post)
        {
            var postDetails = await ApiClient.GetPost(post.Id).ConfigureAwait(false);

            var vm = new PostViewModel(postDetails);
            _dialogService.ShowDialog(vm);

            GetPostPage(1, 25);
        }

        private async void DeletePost(PostListDto post)
        {
            var result = await ApiClient.DeletePost(post.Id).ConfigureAwait(false);
            // TODO: Add popup with delete status info

            GetPostPage(1, 25);
        }
    }
}
