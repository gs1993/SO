using BenchmarkDotNet.Attributes;
using Logic.BoundedContexts.Posts.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BenchmarkTests.APIs
{
    public class RestBenchmarks
    {
        private static readonly HttpClient _httpClient = new();

        [Benchmark]
        public async Task<IReadOnlyList<PostListDto>> RestPostGet100()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var result = await _httpClient
                .GetAsync("http://localhost:5000/api/Post?Offset=100&Limit=100", HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            result.EnsureSuccessStatusCode();

            using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IReadOnlyList<PostListDto>>(contentStream)
                ?? new List<PostListDto>();
        }

        [Benchmark]
        public async Task<IReadOnlyList<PostListDto>> RestPostGet1000()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var result = await _httpClient
                .GetAsync("http://localhost:5000/api/Post?Offset=100&Limit=1000", HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            result.EnsureSuccessStatusCode();

            using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IReadOnlyList<PostListDto>>(contentStream)
                ?? new List<PostListDto>();
        }

        [Benchmark]
        public async Task<IReadOnlyList<PostListDto>> RestPostGet10000()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var result = await _httpClient
                .GetAsync("http://localhost:5000/api/Post?Offset=100&Limit=10000", HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            result.EnsureSuccessStatusCode();

            using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IReadOnlyList<PostListDto>>(contentStream)
                ?? new List<PostListDto>();
        }

        [Benchmark]
        public async Task<PostDetailsDto?> RestPostGetById()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var result = await _httpClient
                .GetAsync("http://localhost:5000/api/Post/8051161", HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            result.EnsureSuccessStatusCode();

            using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<PostDetailsDto>(contentStream);
        }
    }
}
