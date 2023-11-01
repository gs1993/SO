using CSharpFunctionalExtensions;
using ElasticSoDatabase.Indexes;
using ElasticSoDatabase.Utils;
using Logic.Read.Posts.Dtos;
using Microsoft.Extensions.Logging;

namespace ElasticSoDatabase.Services
{
    internal interface IPostSearchService
    {
        Task<Result<SearchResponseWrapper<PostIndex>>> Search(int offset, int limit, IEnumerable<SearchArgs> searchArgs, SortArgs sortArgs, CancellationToken cancellationToken);
    }

    internal class PostSearchService : IPostSearchService
    {
        private readonly Nest.IElasticClient _client;
        private readonly ILogger<PostSearchService> _logger;

        public PostSearchService(Nest.IElasticClient client, ILogger<PostSearchService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Result<SearchResponseWrapper<PostIndex>>> Search(int offset, int limit, 
            IEnumerable<SearchArgs> searchArgs, SortArgs sortArgs, CancellationToken cancellationToken)
        {
            if (offset < 0)
                return Result.Failure<SearchResponseWrapper<PostIndex>>("Offset must be greater than or equal to 0.");
            if (limit < 0)
                return Result.Failure<SearchResponseWrapper<PostIndex>>("Limit must be greater than or equal to 0.");
            if (searchArgs == null)
                return Result.Failure<SearchResponseWrapper<PostIndex>>("Search args must not be null.");
            if (sortArgs == null)
                return Result.Failure<SearchResponseWrapper<PostIndex>>("Sort args must not be null.");

            try
            {
                var validationResult = ValidateSearchArgs(searchArgs);
                if (validationResult.IsFailure)
                    return Result.Failure<SearchResponseWrapper<PostIndex>>(validationResult.Error);

                var searchDescriptor = new Nest.SearchDescriptor<PostIndex>()
                    .From(offset)
                    .Size(limit)
                    .Query(x => BuildQuery(searchArgs))
                    .Sort(x => BuildSort(sortArgs));

                var result = await _client.SearchAsync<PostIndex>(searchDescriptor, cancellationToken);
                if (!result.IsValid)
                {
                    _logger.LogError("An error occurred while searching posts: {Error}", result.OriginalException?.Message ?? result.ServerError?.Error?.Reason);
                    return Result.Failure<SearchResponseWrapper<PostIndex>>("Search error");
                }

                var posts = result.Documents.ToList().AsReadOnly();
                var totalCount = (int)result.Total;

                return Result.Success(new SearchResponseWrapper<PostIndex>(posts, totalCount));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching posts.");
                return Result.Failure<SearchResponseWrapper<PostIndex>>(ex.Message);
            }
        }

        private static Result ValidateSearchArgs(IEnumerable<SearchArgs> searchArgs)
        {
            var postIndexProperties = typeof(PostIndex).GetProperties().ToList();

            foreach (var arg in searchArgs)
            {
                var property = postIndexProperties.SingleOrDefault(x => x.Name == arg.Field);
                if (property == null)
                    return Result.Failure($"Invalid field name: {arg.Field}.");

                if (property.PropertyType == typeof(string) && !IsStringOperation(arg.Operation))
                    return Result.Failure($"Invalid operation {arg.Operation} for string field {arg.Field}.");

                if (property.PropertyType == typeof(int) && !IsIntOperation(arg.Operation))
                    return Result.Failure($"Invalid operation {arg.Operation} for number field {arg.Field}.");

                if (property.PropertyType == typeof(DateTime) && !IsDateOperation(arg.Operation))
                    return Result.Failure($"Invalid operation {arg.Operation} for date field {arg.Field}.");

                if (property.PropertyType == typeof(bool) && !IsBoolOperation(arg.Operation))
                    return Result.Failure($"Invalid operation {arg.Operation} for bool field {arg.Field}.");
            }

            return Result.Success();
        }

        private static bool IsStringOperation(SearchOperation operation)
        {
            return operation switch
            {
                SearchOperation.Equals => true,
                SearchOperation.Contains => true,
                SearchOperation.StartsWith => true,
                _ => false
            };
        }

        private static bool IsIntOperation(SearchOperation operation)
        {
            return operation switch
            {
                SearchOperation.Equals => true,
                SearchOperation.GreaterThan => true,
                SearchOperation.GreaterThanOrEqual => true,
                SearchOperation.LessThan => true,
                SearchOperation.LessThanOrEqual => true,
                _ => false
            };
        }

        private static bool IsDateOperation(SearchOperation operation)
        {
            return operation switch
            {
                SearchOperation.Equals => true,
                SearchOperation.GreaterThan => true,
                SearchOperation.GreaterThanOrEqual => true,
                SearchOperation.LessThan => true,
                SearchOperation.LessThanOrEqual => true,
                _ => false
            };
        }

        private static bool IsBoolOperation(SearchOperation operation)
        {
            return operation switch
            {
                SearchOperation.Equals => true,
                _ => false
            };
        }

        private Nest.QueryContainer BuildQuery(IEnumerable<SearchArgs> searchArgs)
        {
            Nest.QueryContainer? queryContainer = null;
            foreach (var arg in searchArgs)
                queryContainer &= CreateFieldQuery(arg.Field, arg.Operation, arg.Value);

            return queryContainer ?? new Nest.MatchAllQuery();
        }

        private static Nest.QueryContainer CreateFieldQuery(string field, SearchOperation operation, string value)
        {
            return operation switch
            {
                SearchOperation.Equals => (Nest.QueryContainer)new Nest.TermQuery { Field = PropertyNameToCamelCase(field), Value = value },
                SearchOperation.Contains => (Nest.QueryContainer)new Nest.MatchPhraseQuery { Field = PropertyNameToCamelCase(field), Query = value },
                SearchOperation.StartsWith => (Nest.QueryContainer)new Nest.PrefixQuery { Field = PropertyNameToCamelCase(field), Value = value },
                SearchOperation.GreaterThan => (Nest.QueryContainer)new Nest.NumericRangeQuery { Field = PropertyNameToCamelCase(field), GreaterThan = double.Parse(value) },
                SearchOperation.GreaterThanOrEqual => (Nest.QueryContainer)new Nest.NumericRangeQuery { Field = PropertyNameToCamelCase(field), GreaterThanOrEqualTo = double.Parse(value) },
                SearchOperation.LessThan => (Nest.QueryContainer)new Nest.NumericRangeQuery { Field = PropertyNameToCamelCase(field), LessThan = double.Parse(value) },
                SearchOperation.LessThanOrEqual => (Nest.QueryContainer)new Nest.NumericRangeQuery { Field = PropertyNameToCamelCase(field), LessThanOrEqualTo = double.Parse(value) },
                _ => throw new NotSupportedException($"Operation {operation} is not supported for field {field}"),
            };
        }

        private static Nest.SortDescriptor<PostIndex> BuildSort(SortArgs sortArgs)
        {
            return sortArgs.SortDirection == SortDirection.Ascending
                ? new Nest.SortDescriptor<PostIndex>().Ascending(PropertyNameToCamelCase(sortArgs.Field))
                : new Nest.SortDescriptor<PostIndex>().Descending(PropertyNameToCamelCase(sortArgs.Field));
        }

        public static string PropertyNameToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLowerInvariant(str[0]) + str[1..];
        }
    }
}
