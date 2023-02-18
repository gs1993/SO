using HotChocolate.Types;
using Logic.Queries.Posts.Dtos;

namespace Api.GraphQL.Types
{
    public class PostDetailsDtoType : ObjectType<PostDetailsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PostDetailsDto> descriptor)
        {
            descriptor.Field(x => x.Id).Type<IdType>();
            descriptor.Field(x => x.AnswerCount).Type<NonNullType<IntType>>();
            descriptor.Field(x => x.Body).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.ClosedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CommentCount).Type< NonNullType<IntType>>();
            descriptor.Field(x => x.IsClosed).Type<NonNullType<BooleanType>>();
            descriptor.Field(x => x.CommunityOwnedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreationDate).Type<NonNullType<DateTimeType>>();
            descriptor.Field(x => x.LastActivityDate).Type<NonNullType<DateTimeType>>();
            descriptor.Field(x => x.LastEditDate).Type<DateTimeType>();
            descriptor.Field(x => x.LastEditorDisplayName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Score).Type<NonNullType<IntType>>();
            descriptor.Field(x => x.Tags).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Title).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.ViewCount).Type<NonNullType<IntType>>();
        }
    }
}
