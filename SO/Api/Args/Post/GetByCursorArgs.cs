using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public sealed record GetByCursorArgs
    {
        public int? Cursor { get; init; }
        [Required]
        public int Limit { get; init; }
    }
}
