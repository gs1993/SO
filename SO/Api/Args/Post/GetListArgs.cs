using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public sealed record GetListArgs
    {
        [Required]
        public int PageNumber { get; init; }
        [Required]
        public int PageSize { get; init; }
    }
}
