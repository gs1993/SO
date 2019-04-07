using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Utils;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace AdminPanel.Api
{
    public static class ApiClient
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _endpointUrl;

        public static void Init(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public static async Task<Result<PostDetailsDto>> GetPost(int id)
        {
            return await SendRequest<PostDetailsDto>($"Posts/{id}", HttpMethod.Get).ConfigureAwait(false);
        }

        public static async Task<Result<IReadOnlyList<PostListDto>>> GetPostPage(int pageNumber, int pageSize)
        {
            return await SendRequest<IReadOnlyList<PostListDto>>
                ($"Posts/GetPage?pageNumber={pageNumber}&pageSize={pageSize}", HttpMethod.Get).ConfigureAwait(false);
        }

        public static async Task<Result> DeletePost(int id)
        {
            return await SendRequest<string>($"Posts/{id}", HttpMethod.Delete).ConfigureAwait(false);
        }

        public static async Task<Result> UpdatePost(PostListDto post)
        {
            return await SendRequest<string>($"Posts", HttpMethod.Post).ConfigureAwait(false);
        }

        public static async Task<Result> ClosePost(int id)
        {
            return await SendRequest<string>($"Posts/Close", HttpMethod.Post, new { id }).ConfigureAwait(false);
        }


        public static async Task<Result<IReadOnlyList<LastUserDto>>> GetLastCreatedUsersAsync(int size = 10)
        {
            return await SendRequest<IReadOnlyList<LastUserDto>>
                ($"Users/GetLast?size={size}", HttpMethod.Get).ConfigureAwait(false);
        }


        private static async Task<Result<T>> SendRequest<T>(string url, HttpMethod method, object content = null)
            where T : class
        {
            try
            {
                var request = new HttpRequestMessage(method, $"{_endpointUrl}/{url}");
                if (content != null)
                    request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var message = await _client.SendAsync(request).ConfigureAwait(false);
                var response = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (message.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception(response);

                var envelope = JsonConvert.DeserializeObject<Envelope<T>>(response);
                
                if (!message.IsSuccessStatusCode)
                    return Result.Fail<T>(envelope.ErrorMessage);

                var result = envelope.Result;
                if (result == null && typeof(T) == typeof(string))
                    result = string.Empty as T;

                return Result.Ok(result);
            }
            catch (Exception e)
            {
                return Result.Fail<T>(e.Message);
            }
        }
    }
}
