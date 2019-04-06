using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
        public ICommand ClosePostCommand { get; }


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
            var postListResult = await ApiClient.GetPostPage(1, 25);
            if (postListResult.IsFailure)
                return; //TODO add error message

            PostList = postListResult.Value;
            Notify(nameof(PostList));
        }

        private async void UpdatePost(PostListDto post)
        {
            var postDetailsResult = await ApiClient.GetPost(post.Id).ConfigureAwait(false);

            if (postDetailsResult.IsFailure)
                return; //TODO: Add error message

            var vm = new PostViewModel(postDetailsResult.Value);
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
