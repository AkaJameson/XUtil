using DotNet.Util.Core.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.HttpHelper
{
    internal class Request:IRequest
    {
        private readonly HttpClient _httpClient;
        public Request(double? Timeout = null)
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // 忽略 SSL 认证
            });
            if(Timeout.HasValue)
                _httpClient.Timeout = TimeSpan.FromSeconds(Timeout.Value);
        }
        public void SetBaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
        }
        private string BuildUrlWithParameters(string url, IDictionary<string, string>? parameters)
        {
            if (parameters != null && parameters.Any())
            {
                var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}"));
                url = url.Contains("?") ? $"{url}&{queryString}" : $"{url}?{queryString}";
            }
            return url;
        }

        private void SetHeaders(IDictionary<string, string>? headers)
        {
            // 清空现有请求头
            _httpClient.DefaultRequestHeaders.Clear();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }
        #region Get Method
        public async Task<T> GetAsync<T>(string url) where T : class
        {
            SetHeaders(null);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stringData = await response.Content.ReadAsStringAsync();
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
            
        }

        public T Get<T>(string url) where T : class
        {
            SetHeaders(null);
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var stringData = response.Content.ReadAsStringAsync().Result;
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public async Task<T> GetAsync<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stringData = await response.Content.ReadAsStringAsync();
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public T Get<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var stringData = response.Content.ReadAsStringAsync().Result;
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public async Task<string> GetStringAsync(string url)
        {
            SetHeaders(null);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public string GetString(string url)
        {
            SetHeaders(null);
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> GetStringAsync(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers)
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public string GetString(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers)
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        #endregion

        #region Post Method
        public async Task<T> PostAsync<T>(string url) where T : class
        {
            SetHeaders(null);
            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
            var stringData = await response.Content.ReadAsStringAsync();
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public T Post<T>(string url) where T : class
        {
            SetHeaders(null);
            var response = _httpClient.PostAsync(url, null).Result;
            response.EnsureSuccessStatusCode();
            var stringData = response.Content.ReadAsStringAsync().Result;
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public async Task<T> PostAsync<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, object? body) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var data = JsonConvert.SerializeObject(body);
            HttpContent content = body is null ? null : new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url,content);
            response.EnsureSuccessStatusCode();
            var stringData = await response.Content.ReadAsStringAsync();
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public T Post<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, object? body ) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var data = JsonConvert.SerializeObject(body);
            HttpContent content = body is null ? null : new StringContent(data, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();
            var stringData = response.Content.ReadAsStringAsync().Result;
            //使用Newtonsoft 忽略大小写
            return stringData.ConvertToObject<T>();
        }

        public async Task<string> PostStringAsync(string url)
        {
            SetHeaders(null);
            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public string PostString(string url)
        {
            var response = _httpClient.PostAsync(url, null).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> PostStringAsync(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, object? body)
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var data = JsonConvert.SerializeObject(body);
            HttpContent content = body is null ? null : new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public string PostString(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers,object body)
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var data = JsonConvert.SerializeObject(body);
            HttpContent content = body is null ? null : new StringContent(data, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
        #endregion
    }
}
