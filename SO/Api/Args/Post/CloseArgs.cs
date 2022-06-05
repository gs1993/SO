using System.ComponentModel.DataAnnotations;

namespace Api.Args.Post
{
    public record CloseArgs
    {
        [Required]
        public int Id { get; init; }
    }
}
