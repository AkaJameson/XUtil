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
        public static Request Instance { get; } = new Request();
        public Request()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // 忽略 SSL 认证
            });
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
        private HttpContent BuildHttpContent(IDictionary<string, string> body, string contentType)
        {
            HttpContent content;
            if (contentType == "application/json")
            {
                // 序列化为 JSON
                var json = JsonConvert.SerializeObject(body);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            else
            {
                // 默认使用 x-www-form-urlencoded
                content = new FormUrlEncodedContent(body);
            }

            return content;
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
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public T Get<T>(string url) where T : class
        {
            SetHeaders(null);
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<T>().Result;
        }

        public async Task<T> GetAsync<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public T Get<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<T>().Result;
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
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public T Post<T>(string url) where T : class
        {
            SetHeaders(null);
            var response = _httpClient.PostAsync(url, null).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<T>().Result;
        }

        public async Task<T> PostAsync<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, IDictionary<string, string>? body) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            string contentType;
            if (headers is not null&& headers["Content-Type"]!=null)
            {
                contentType = headers["Content-Type"];
            }
            else
            {
                contentType = "application/json";
            }
            var content = BuildHttpContent(body, contentType);
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public T Post<T>(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, IDictionary<string, string>? body) where T : class
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);
            string contentType;
            if (headers is not null && headers["Content-Type"] != null)
            {
                contentType = headers["Content-Type"];
            }
            else
            {
                contentType = "application/json";
            }
            var content = BuildHttpContent(body, contentType);

            var response = _httpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<T>().Result;
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

        public async Task<string> PostStringAsync(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, IDictionary<string, string> body)
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);

            string contentType;
            if (headers is not null && headers["Content-Type"] != null)
            {
                contentType = headers["Content-Type"];
            }
            else
            {
                contentType = "application/json";
            }
            var content = BuildHttpContent(body, contentType);

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public string PostString(string url, IDictionary<string, string>? parameters, IDictionary<string, string>? headers, IDictionary<string, string> body)
        {
            url = BuildUrlWithParameters(url, parameters);
            SetHeaders(headers);

            string contentType;
            if (headers is not null && headers["Content-Type"] != null)
            {
                contentType = headers["Content-Type"];
            }
            else
            {
                contentType = "application/json";
            }
            var content = BuildHttpContent(body, contentType);
            var response = _httpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }


        #endregion
    }
}
