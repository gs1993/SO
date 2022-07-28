using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Logic.BoundedContexts.Users.Entities
{
    public class VoteSummary : ValueObject
    {
        protected VoteSummary() { }
        private VoteSummary(int upVotes, int downVotes)
        {
            UpVotes = upVotes;
            DownVotes = downVotes;
        }


        public int VoteCount => UpVotes + DownVotes;
        public int UpVotes { get; protected set; }
        public int DownVotes { get; protected set; }


        public static Result<VoteSummary> Create(int upVotes, int downVotes)
        {
            if (upVotes < 0)
                return Result.Failure<VoteSummary>("UpVotes cannot be less than 0");

            if (downVotes < 0)
                return Result.Failure<VoteSummary>("DownVotes cannot be less than 0");

            return Result.Success(new VoteSummary(upVotes, downVotes));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UpVotes;
            yield return DownVotes;
        }
    }
}
