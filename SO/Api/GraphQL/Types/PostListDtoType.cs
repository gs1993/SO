using HotChocolate.Types;
using Logic.BoundedContexts.Posts.Dtos;

namespace Api.GraphQL.Types
{
    public class PostListDtoType : ObjectType<PostListDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PostListDto> descriptor)
        {
            descriptor.Field(x => x.Id).Type<IdType>();
            descriptor.Field(x => x.AnswerCount).Type<NonNullType<IntType>>();
            descriptor.Field(x => x.IsClosed).Type< NonNullType<BooleanType>>();
            descriptor.Field(x => x.CommentCount).Type< NonNullType<IntType>>();
            descriptor.Field(x => x.CreationDate).Type< NonNullType<DateTimeType>>();
            descriptor.Field(x => x.Score).Type< NonNullType<IntType>>();
            descriptor.Field(x => x.Title).Type< NonNullType<StringType>>();
            descriptor.Field(x => x.Body).Type< NonNullType<StringType>>();
            descriptor.Field(x => x.ViewCount).Type< NonNullType<IntType>>();
        }
    }
}
