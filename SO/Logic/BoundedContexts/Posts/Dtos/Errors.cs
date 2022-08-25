using Logic.Utils;

namespace Logic.BoundedContexts.Posts.Dtos
{

    public static class Errors
    {
        public static class Post
        {
            public static Error CommentIsRequired() => new("Comment cannot be empty");
            public static Error AlreadyClosed() => new("Post already closed");
        }

        public static class User
        {
            public static Error AlreadyDeleted() => new("User was already deleted");
            public static Error NotExists(int id) => new($"User with id {id} not exists");
        }

        public static class General
        {
            public static Error ValueIsRequired(string name) => new($"{name} is required");
            public static Error InvalidLength(string name) => new($"{name} length is invalid");
            public static Error InvalidValue(string name) => new($"{name} value is invalid");
        }
    }
}
