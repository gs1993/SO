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
            GetPostPage(1, PageSize);
        }
        
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
    }
}
