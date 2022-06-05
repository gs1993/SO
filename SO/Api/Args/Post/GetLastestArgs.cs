using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public record GetLastestArgs
    {
        [Required]
        public int Size { get; init; }
    }
}
