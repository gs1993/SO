using Logic.Utils;

namespace Logic.BoundedContexts.Posts.Entities
{
    public class PostType : BaseEntity
    {
        public static readonly PostType Question = new("Question");
        public static readonly PostType Answer = new("Answer");

        public string Type { get; }

        private PostType() { }
        private PostType(string type)
        {
            Type = type;
        }
    }
}
