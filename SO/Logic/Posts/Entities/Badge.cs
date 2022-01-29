using Logic.Models;
using System;

namespace Logic.Posts.Entities
{
    public partial class Badges : BaseEntity
    {
        public string Name { get; private set; }
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
    }
}
