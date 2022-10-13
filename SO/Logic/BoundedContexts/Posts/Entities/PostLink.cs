using Logic.Utils.Db;

namespace Logic.BoundedContexts.Posts.Entities
{
    public class PostLink : BaseEntity
    {
        public int PostId { get; private set; }
        public int RelatedPostId { get; private set; }
        public int LinkTypeId { get; private set; }
    }
}
