using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public sealed record GetListArgs
    {
        [Required]
        public int Offset { get; init; }
        [Required]
        public int Limit { get; init; }
    }
}
