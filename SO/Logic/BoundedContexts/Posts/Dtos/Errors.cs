using Logic.Utils;

namespace Logic.BoundedContexts.Posts.Dtos
{

    public static class Errors
    {
        public static class Post
        {
            public static Error CommentIsRequired() => new("Comment cannot be empty");
        }

        public static class General
        {
            public static Error ValueIsRequired(string name) => new($"{name} is required");
            public static Error InvalidLength(string name) => new($"{name} length is invalid");
        }
    }
}
