using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement.FeatureFilters;
using System;
using System.Threading.Tasks;

namespace Api.Utils
{
    public class HttpContextTargetingContextAccessor : ITargetingContextAccessor
    {
        private const string UserIdHeaderName = "UserId";
        private const string UserGroupsHeaderName = "GroupName1,GroupName2";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ValueTask<TargetingContext> GetContextAsync()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            var userId = string.Empty;
            if (httpContext.Response.Headers.TryGetValue(UserIdHeaderName, out var userIdValue)) // Example without user auth
                userId = userIdValue.ToString();

            var userGroups = Array.Empty<string>();
            if (httpContext.Response.Headers.TryGetValue(UserGroupsHeaderName, out var userGroupsValue)) // Example without user auth
                userGroups = userGroupsValue.ToString().Split(",");

            return new ValueTask<TargetingContext>(new TargetingContext
            {
                UserId = userId,
                Groups = userGroups
            });
        }
    }
}
