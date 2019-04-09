using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Logic.Models.Users
{
    public class VoteSummary : ValueObject
    {
        protected VoteSummary()
        {

        }
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
                return Result.Fail<VoteSummary>("UpVotes cannot be less than 0");

            if (downVotes < 0)
                return Result.Fail<VoteSummary>("DownVotes cannot be less than 0");

            return Result.Ok(new VoteSummary(upVotes, downVotes));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UpVotes;
            yield return DownVotes;
        }
    }
}
