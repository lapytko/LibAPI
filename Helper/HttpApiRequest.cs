using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LibAPI.Extensions;
using LibAPI.Models.ResponseResult;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LibAPI.Helper
{
    public class HttpApiRequest
    {
        public string BaseUrl { get; set; }
        public string SystemSection { get; set; }
        public bool LogWithoutResponse { get; set; } = false;

        private static readonly string
            PathLogging = Path.Combine(Directory.GetCurrentDirectory(), "Utils\\Logging");

        public async Task<ResponseResult> Get<T>(string uri, string token = null,
            Dictionary<string, string> headers = null, Dictionary<string, string> logInfo = null) where T : class
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
                client.DefaultRequestHeaders.Accept.Clear();
                if (token != null)
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                if (headers != null)
                    foreach (var (key, value) in headers)
                        client.DefaultRequestHeaders.Add(key, value);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"{BaseUrl}/{uri}");

                await SaveFairLogging("GET", $"{BaseUrl}/{uri}", client, response);

                if (!response.IsSuccessStatusCode)
                    return new ResponseResult(false, ((int)response.StatusCode).ToString());
                var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawResponse = readTask.GetAwaiter().GetResult();

                if (typeof(T) == typeof(string))
                {
                    return new ResponseResult(true, ((int)response.StatusCode).ToString(), rawResponse);
                }

                var obj = JsonConvert.DeserializeObject<T>(rawResponse);
                return new ResponseResult(true, ((int)response.StatusCode).ToString(), obj);
            }
            catch (Exception e)
            {
                return new ResponseResult(false, "500", e.Message);
            }
        }

        public async Task<ResponseResult> Get<T, TError>(string uri, string token = null,
            Dictionary<string, string> headers = null, Dictionary<string, string> logInfo = null) where T : class
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
                client.DefaultRequestHeaders.Accept.Clear();
                if (token != null)
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                if (headers != null)
                    foreach (var (key, value) in headers)
                        client.DefaultRequestHeaders.Add(key, value);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"{BaseUrl}/{uri}");
                await SaveFairLogging("GET", $"{BaseUrl}/{uri}", client, response);
                var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawResponse = readTask.GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var obj = JsonConvert.DeserializeObject<T>(rawResponse);
                    return new ResponseResult(true, ((int)response.StatusCode).ToString(), obj);
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<TError>(rawResponse);
                    return new ResponseResult(false, ((int)response.StatusCode).ToString(), obj);
                }
            }
            catch (Exception e)
            {
                return new ResponseResult(false, "500", e.Message);
            }
        }

        public async Task<ResponseResult> Post<T>(string uri, object data = null, string token = null,
            Dictionary<string, string> headers = null, Dictionary<string, string> logInfo = null) where T : class
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
                client.DefaultRequestHeaders.Accept.Clear();
                if (token != null)
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                if (headers != null)
                    foreach (var (key, value) in headers)
                        client.DefaultRequestHeaders.Add(key, value);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json = data != null ? JsonConvert.SerializeObject(data, serializerSettings) : "";
                var response = await client.PostAsync($"{BaseUrl}/{uri}",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                await SaveFairLogging("POST", $"{BaseUrl}/{uri}", client, response, json);

                if (!response.IsSuccessStatusCode)
                    return new ResponseResult(false, ((int)response.StatusCode).ToString());

                var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawResponse = readTask.GetAwaiter().GetResult();
                var obj = JsonConvert.DeserializeObject<T>(rawResponse);
                return new ResponseResult(true, "Ok", obj);
            }
            catch (Exception e)
            {
                return new ResponseResult(false, e.Message);
            }
        }

        public async Task<ResponseResult> Post<T, TError>(string uri, object data = null, string token = null,
            Dictionary<string, string> headers = null, Dictionary<string, string> logInfo = null)
            where T : class where TError : class
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
                client.DefaultRequestHeaders.Accept.Clear();
                if (token != null)
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                if (headers != null)
                    foreach (var (key, value) in headers)
                        client.DefaultRequestHeaders.Add(key, value);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json = data != null ? JsonConvert.SerializeObject(data, serializerSettings) : "";
                var response = await client.PostAsync($"{BaseUrl}/{uri}",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                await SaveFairLogging("POST", $"{BaseUrl}/{uri}", client, response, json);

                var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawResponse = readTask.GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    return rawResponse.JsonIsObject()
                        ? new ResponseResult(true, "Ok", JsonConvert.DeserializeObject<T>(rawResponse))
                        : new ResponseResult(true, "Ok", rawResponse);
                }

                return new ResponseResult(false, ((int)response.StatusCode).ToString(),
                    rawResponse.JsonIsObject() ? JsonConvert.DeserializeObject<TError>(rawResponse) : rawResponse);
            }
            catch (Exception e)
            {
                return new ResponseResult(false, e.Message);
            }
        }

        private async Task SaveFairLogging(string type, string url, HttpClient client,
            HttpResponseMessage response, string body = null, Dictionary<string, string> logInfo = null)
        {
            var logStr = string.Empty;
            var dateNow = DateTime.Now;
            logStr += $"Url ({type}): {url}\r\n{dateNow.ToLongDateString()} {dateNow.ToLongTimeString()}\r\n\r\n";
            logStr += $"Раздел: {SystemSection}\r\n";
            logStr += string.Join("\r\n", logInfo?.Select(x => $"{x.Key} - {x.Value}") ?? new List<string>());

            logStr += body != null ? $"Body: {body}\r\n" : string.Empty;
            logStr +=
                $"Headers: {string.Join("\r\n", client.DefaultRequestHeaders.Select(x => $"{x.Key} - {string.Join(", ", x.Value)}"))}\r\n\r\n";
            logStr += $"Result status: {(int)response.StatusCode} ({response.StatusCode.ToString()})\r\n";
            logStr +=
                $"Result response: " +
                $"{(!LogWithoutResponse ? response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult() : "Hidden")}\r\n";
            if (!Directory.Exists(PathLogging))
            {
                Directory.CreateDirectory(PathLogging);
                var strCmdText = $"ForFiles /p \"{PathLogging}\" /s /d -60 /c \"cmd /c del /q @file\"";
                System.Diagnostics.Process.Start("cmd.exe", strCmdText);
            }

            await File.WriteAllTextAsync($"{PathLogging}\\{type} {SystemSection} {dateNow:dd-MM-yyyy HH-mm-ss}-{Guid.NewGuid()}.txt", logStr);
        }
    }
}