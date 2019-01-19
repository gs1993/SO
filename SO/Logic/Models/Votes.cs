using System;

namespace Logic.Models
{
    public partial class Votes : Entity
    {
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public int? BountyAmount { get; set; }
        public int VoteTypeId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
