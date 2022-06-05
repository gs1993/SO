using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public record GetArgs
    {
        [Required]
        public int Id { get; init; }
    }
}
