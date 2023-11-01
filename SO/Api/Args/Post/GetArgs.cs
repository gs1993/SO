using Logic.Read.Posts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public sealed record GetArgs
    {
        [Required]
        public int Offset { get; init; }
        [Required]
        public int Limit { get; init; }
        public SearchArgs[] SearchArgs { get; init; }
        public SortArgs SortArgs { get; init; }
    }
}
