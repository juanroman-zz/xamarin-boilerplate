using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Request
{
    public class RequestService : IRequestService
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RequestService()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _jsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            var httpClient = InitializeHttpClient(token);
            var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _jsonSerializerSettings));

            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "")
        {
            return PostAsync<TResult, TResult>(uri, data, token);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            var httpClient = InitializeHttpClient(token);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _jsonSerializerSettings));
            var response = await httpClient.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var json = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "")
        {
            return PutAsync<TResult, TResult>(uri, data, token);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            var httpClient = InitializeHttpClient(token);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _jsonSerializerSettings));
            var response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var json = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings));
        }

        private HttpClient InitializeHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(token))
            {
                var scheme = IsEmail(token) ? "Email" : "Bearer";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
            }

            return httpClient;
        }

        private bool IsEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (HttpStatusCode.Forbidden == response.StatusCode || HttpStatusCode.Unauthorized == response.StatusCode)
                {
                    throw new UnauthorizedAccessException(content);
                }

                throw new HttpRequestException(content);
            }
        }
    }
}
