using HotChocolate.Types;
using Logic.BoundedContexts.Posts.Dtos;

namespace Api.GraphQL.Types
{
    public class PostListDtoType : ObjectType<PostListDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PostListDto> descriptor)
        {
            descriptor.Field(x => x.Id).Type<IdType>();
            descriptor.Field(x => x.Title).Type<StringType>();
            descriptor.Field(x => x.ShortBody).Type<StringType>();
        }
    }
}
