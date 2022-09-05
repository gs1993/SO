using Logic.Utils;

namespace Logic.BoundedContexts.Posts.Entities
{
    public partial class Badge : BaseEntity
    {
        public string Name { get; private set; }
        public int UserId { get; private set; }
    }
}
