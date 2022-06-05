using System.ComponentModel.DataAnnotations;

namespace Api.Args.User
{
    public sealed record GetArgs
    {
        [Required]
        public int Id { get; init; }
    }
}
