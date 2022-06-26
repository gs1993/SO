using Logic.Models;
using Logic.Users.Entities;
using System;

namespace Logic.Posts.Entities
{
    public class Vote : BaseEntity
    {
        public int PostId { get; private set; }
        public int? UserId { get; private set; }
        public int? BountyAmount { get; private set; }
        public int VoteTypeId { get; private set; }

        protected Vote() { }
        public Vote(Post post, User user, int bountyAmount)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            PostId = post.Id;
            UserId = user.Id;
            BountyAmount = bountyAmount;
        }
    }
}
