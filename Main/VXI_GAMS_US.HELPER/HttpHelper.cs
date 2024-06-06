using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using VXI_GAMS_US.VIEWS.View;

namespace VXI_GAMS_US.HELPER
{
    
    public static class HttpHelper<T> where T : class
    {
        public static async Task<T> GetApiViewModelAccount(string completeUrl, bool enclose = false)
        {
            try
            {
                var url = $"{completeUrl}";
                url = await HttpHelper.GetAsync(url);
                url = enclose ? url.Contains("\"Table\\\": [") ? url : $"[{url}]" : url;
                return JsonHelper<T>.Deserialize(url);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }
        public static async Task<T> GetApiViewModelAccount(string webapi, string hrid, bool enclose = false)
        {
            try
            {
                var url = $"{webapi}{hrid}";
                url = await HttpHelper.GetAsync(url);
                url = enclose ? url.Contains("\"Table\\\": [") ? url : $"[{url}]" : url;
                return JsonHelper<T>.Deserialize(url);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }
        public static async Task<T> GetApiViewModelAccount(string webapi, List<HridRegionViewModel> hrids, bool enclose = false)
        {
            try
            {
                var url = $"{webapi}";
                url = await HttpHelper.GetAsync(url, hrids);
                url = enclose ? $"[{url}]" : url;
                return JsonHelper<T>.Deserialize(url);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }

        public static async Task<T> GetApiViewModelAccount2(string webapi, List<HridRegionViewModel> hrids)
        {
            try
            {
                var url = $"{webapi}";
                url = await HttpHelper.GetAsync(url, hrids);
                return JsonConvert.DeserializeObject<T>(url);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }

        public static async Task<T> GetApiViewModelAccount(string webapi, string hrid, string region, bool enclose = false)
        {
            try
            {
                var url = $"{webapi}{hrid}/{region}";
                url = await HttpHelper.GetAsync(url);
                url = enclose ? $"[{url}]" : url;
                return JsonHelper<T>.Deserialize(url);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }
    }
    /// <summary>
    /// HTTP Helper
    /// </summary>
    public class HttpHelper
    {
        private const string jsonMediaType = "application/json";
        private const string xmlMediaType = "text/xml";

        public static async Task<string> GetAsync(string url)
        {
            try
            {
                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonMediaType));
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.ReasonPhrase);
                    }

                    var data = await response.Content.ReadAsStringAsync();

                    return response.Content.Headers.ContentType.MediaType.Equals(xmlMediaType) ?
                        XElement.Parse(data).Value : data;
                }
            }
            catch (Exception exception)
            {
                var error = ExceptionHandler.GetMessages(exception);
                Console.WriteLine(error);
            }
            return null;
        }
        public static async Task<string> GetAsync(string url, object postData)
        {
            try
            {
                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonMediaType));
                    var _obj = JsonConvert.SerializeObject(postData);
                    var buffer = Encoding.UTF8.GetBytes(_obj);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue(jsonMediaType);
                    var response = await client.PostAsync(url, byteContent);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.ReasonPhrase);
                    }

                    var data = await response.Content.ReadAsStringAsync();

                    return response.Content.Headers.ContentType.MediaType.Equals(xmlMediaType) ?
                        XElement.Parse(data).Value : data;
                }
            }
            catch (Exception exception)
            {
                var error = ExceptionHandler.GetMessages(exception);
                Console.WriteLine(error);
            }
            return null;
        }

        public static async Task<string> GetImageAsync(string url, string fallbackImage)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["VRS_USERNAME"], ConfigurationManager.AppSettings["VRS_PASSWORD"]);
                    var data = await client.DownloadDataTaskAsync(url);
                    if (data.Length > 0)
                    {
                        var str = Convert.ToBase64String(data);
                        return $"data:image/jpeg;base64,{str}";
                    }
                }
                return fallbackImage;
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
                return fallbackImage;
            }
        }

        public static string GetImage(string url, string fallbackImage)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["VRS_USERNAME"], ConfigurationManager.AppSettings["VRS_PASSWORD"]);
                    var data = client.DownloadData(url);
                    if (data.Length > 0)
                    {
                        var str = Convert.ToBase64String(data);
                        return $"data:image/jpeg;base64,{str}";
                    }
                }
                return fallbackImage;
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
                return fallbackImage;
            }
        }
    }

}
