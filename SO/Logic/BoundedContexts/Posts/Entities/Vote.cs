using CSharpFunctionalExtensions;
using Logic.BoundedContexts.Users.Entities;
using Logic.Utils.Db;
using System;

namespace Logic.BoundedContexts.Posts.Entities
{
    public class Vote : Entity<int>
    {
        public int PostId { get; private set; }
        public int? UserId { get; private set; }
        public int? BountyAmount { get; private set; }
        public int VoteTypeId { get; private set; }

        protected Vote() { }
        public Vote(Post post, User user, int bountyAmount)
        {
            ArgumentNullException.ThrowIfNull(post);
            ArgumentNullException.ThrowIfNull(user);

            PostId = post.Id;
            UserId = user.Id;
            BountyAmount = bountyAmount;
        }
    }
}
