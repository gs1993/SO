using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Args.User
{
    public sealed record GetLastArgs
    {
        [Required]
        [DefaultValue(20)]
        public int Size { get; init; }
    }
}
