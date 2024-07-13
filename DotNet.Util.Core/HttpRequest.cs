using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil
{
    public class HttpRequest:IDisposable
    {
        private readonly HttpClient _httpClient;

        public HttpRequest()
        {
            _httpClient = new HttpClient();
        }

        public void AddDefaultRequestHeader(string name, string value)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains(name))
            {
                _httpClient.DefaultRequestHeaders.Add(name, value);
            }
        }

        public void SetDefaultRequestHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.Remove(name);
            _httpClient.DefaultRequestHeaders.Add(name, value);
        }

        public async Task<dynamic> GetAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            return await HandleResponse(response);
        }

        public async Task<dynamic> GetAsync(string uri, Dictionary<string, string> queryParams)
        {
            var query = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;
            var fullUri = $"{uri}?{query}";

            var response = await _httpClient.GetAsync(fullUri);
            return await HandleResponse(response);
        }

        public async Task<dynamic> PostAsync<TRequest>(string uri, TRequest data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            return await HandleResponse(response);
        }

        public async Task<dynamic> PutAsync<TRequest>(string uri, TRequest data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);
            return await HandleResponse(response);
        }

        public async Task<dynamic> DeleteAsync(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            return await HandleResponse(response);
        }

        private async Task<dynamic> HandleResponse(HttpResponseMessage response)
        {
            dynamic apiResponse = new ExpandoObject();
            apiResponse.IsSuccess = response.IsSuccessStatusCode;
            apiResponse.StatusCode = response.StatusCode;

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                apiResponse.Data = JsonConvert.DeserializeObject<ExpandoObject>(content);
            }
            else
            {
                apiResponse.ErrorMessage = content;
            }

            return apiResponse;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }


    }
}
