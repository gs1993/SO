namespace Logic.Utils
{

    public static class Errors
    {
        public static class Posts
        {
            public static Error CommentIsRequired() => new("Comment cannot be empty");
            public static Error AlreadyClosed() => new("Post already closed");
            public static Error DoesNotExists(int id) => new($"Post with id {id} does not exist");
            public static Error InappropriatePostContent() => new("Inappropriate post content");
        }

        public static class Users
        {
            public static Error AlreadyDeleted() => new("User was already deleted");
            public static Error DoesNotExists(int id) => new($"User with id {id} does not exist");
        }

        public static class General
        {
            public static Error ValueIsRequired(string name) => new($"{name} is required");
            public static Error InvalidLength(string name) => new($"{name} length is invalid");
            public static Error InvalidValue(string name) => new($"{name} value is invalid");
        }
    }
}
