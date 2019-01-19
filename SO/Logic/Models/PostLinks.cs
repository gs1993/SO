using System;

namespace Logic.Models
{
    public partial class PostLinks : Entity
    {
        public DateTime CreationDate { get; set; }
        public int PostId { get; set; }
        public int RelatedPostId { get; set; }
        public int LinkTypeId { get; set; }
    }
}
