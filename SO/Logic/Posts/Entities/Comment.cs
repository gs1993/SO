﻿using Logic.Models;
using System;

namespace Logic.Posts.Entities
{
    public partial class Comment : BaseEntity
    {
        public int? Score { get; private set; }
        public string Text { get; private set; }
        public int? UserId { get; private set; }
    }
}
