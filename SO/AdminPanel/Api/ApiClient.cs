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
    public class ApiClient
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _endpointUrl;

        public static void Init(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public static async Task<PostDetailsDto> GetPost(int id)
        {
            var result = await SendRequest<PostDetailsDto>($"?id={id}", HttpMethod.Get).ConfigureAwait(false);
            if (result.IsFailure)
            {
                // TODO: add fail user message
            }

            return result.Value;
        }


        private static async Task<Result<T>> SendRequest<T>(string url, HttpMethod method, object content = null)
            where T : class
        {
            var request = new HttpRequestMessage(method, $"{_endpointUrl}/{url}");
            if (content != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            
            var message = await _client.SendAsync(request).ConfigureAwait(false);
            var response = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            var envelope = JsonConvert.DeserializeObject<Envelope<T>>(response);

            if (message.StatusCode == HttpStatusCode.InternalServerError)
                throw new Exception(envelope.ErrorMessage);

            if (!message.IsSuccessStatusCode)
                return Result.Fail<T>(envelope.ErrorMessage);

            var result = envelope.Result;
            if (result == null && typeof(T) == typeof(string))
                result = string.Empty as T;

            return Result.Ok(result);
        }
    }
}
