using Logic.Models;
using System;

namespace Logic.Posts.Entities
{
    public partial class PostLink : BaseEntity
    {
        public DateTime CreationDate { get; private set; }
        public int PostId { get; private set; }
        public int RelatedPostId { get; private set; }
        public int LinkTypeId { get; private set; }
    }
}
