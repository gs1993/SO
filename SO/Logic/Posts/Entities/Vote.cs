﻿using Logic.Models;
using System;

namespace Logic.Posts.Entities
{
    public partial class Vote : BaseEntity
    {
        public int PostId { get; private set; }
        public int? UserId { get; private set; }
        public int? BountyAmount { get; private set; }
        public int VoteTypeId { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}