using System.ComponentModel.DataAnnotations;

namespace Api.Args.User
{
    public sealed record GetLastArgs
    {
        [Required]
        public int Size { get; init; }
    }
}
