using Logic.Utils;

namespace Logic.BoundedContexts.Posts.Entities
{
    public class PostType : BaseEntity
    {
        public static readonly PostType Question = new(1, "Question");
        public static readonly PostType Answer = new(2, "Answer");

        public string Type { get; }

        protected PostType() { }
        private PostType(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
