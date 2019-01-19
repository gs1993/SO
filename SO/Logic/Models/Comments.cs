using System;

namespace Logic.Models
{
    public partial class Comments : Entity
    {
        public DateTime CreationDate { get; set; }
        public int PostId { get; set; }
        public int? Score { get; set; }
        public string Text { get; set; }
        public int? UserId { get; set; }
    }
}
