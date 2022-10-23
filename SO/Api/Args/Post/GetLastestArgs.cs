using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public sealed record GetLastestArgs
    {
        [Required]
        public int Size { get; init; }
    }
}
