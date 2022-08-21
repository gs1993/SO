using Logic.Utils;

namespace Logic.BoundedContexts.Posts.Entities
{
    public partial class Comment : BaseEntity
    {
        public int? Score { get; private set; }
        public string Text { get; private set; }
        public int? UserId { get; private set; }

        protected Comment() { }
        public Comment(int userId, string text)
        {
            UserId = userId;
            Text = text;
            Score = 0;
        }
    }
}
