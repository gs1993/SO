﻿using Logic.Models;

namespace Logic.Posts.Entities
{
    public partial class Badge : BaseEntity
    {
        public string Name { get; private set; }
        public int UserId { get; private set; }
    }
}
