using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TB.AspNetCore.Infrastructrue.Logs;

namespace TB.AspNetCore.Infrastructrue.Utils.Http
{
    public class HttpUtility
    {
        public static string GetString(string url)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(handler))
            {
                return client.GetStringAsync(url).GetAwaiter().GetResult();//.GetAwaiter().GetResult();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="cert"></param>
        /// <param name="contentType">application/x-www-form-urlencoded,multipart/form-data,application/json</param>
        /// <returns></returns>
        public static string PostString(string url, string postData, string contentType = "application/x-www-form-urlencoded", X509Certificate2 cert = null,
            string headerAuthorization = "",
            string charset = "utf-8",
            string contentCharset = "utf-8")
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;
                if (cert != null)
                {
                    handler.ClientCertificates.Add(cert);
                }
                handler.ServerCertificateCustomValidationCallback = (x, y, z, m) =>
                {
                    return true;
                };
                handler.AllowAutoRedirect = true;
                handler.UseCookies = true;
                using (HttpClient client = new HttpClient(handler))
                {
                    var h = client.DefaultRequestHeaders;
                    h.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

                    StringContent content = new StringContent(postData, Encoding.GetEncoding(charset), contentType);
                    if (!string.IsNullOrEmpty(headerAuthorization))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", headerAuthorization);
                    }

                    var c = client.PostAsync(url, content);
                    c.Wait();
                    var response = c.Result;

                    var bytes = response.Content.ReadAsByteArrayAsync().Result;

                    var encoding = System.Text.Encoding.GetEncoding(contentCharset);
                    var str = encoding.GetString(bytes);
                    return str;
                }
            }
            catch (Exception ex) { throw new Exception(url + " :" + ex.Message); }
        }

        public static string Upload(string url, string filePath)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(handler))
            {
                MultipartFormDataContent content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var file = new System.IO.FileInfo(filePath);
                content.Add(new StreamContent(file.OpenRead()), "media", file.Name);
                using (var response = client.PostAsync(url, content).GetAwaiter().GetResult())
                {
                    return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }
        public static string Upload(string url, string fileName, byte[] bytes)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(handler))
            {
                MultipartFormDataContent content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                {
                    content.Add(new StreamContent(ms), "media", fileName);
                    using (var response = client.PostAsync(url, content).GetAwaiter().GetResult())
                    {
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }
            }
        }
        public static Stream DownStream(string url)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(handler))
            {
                return client.GetStreamAsync(url).GetAwaiter().GetResult();
            }
        }
        public static byte[] DownBytes(string url)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(handler))
            {
                try
                {
                    using (var stream = client.GetStreamAsync(url).GetAwaiter().GetResult())
                    {
                        var ms = new MemoryStream();
                        stream.CopyTo(ms);
                        byte[] bytes = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(bytes, 0, bytes.Length);
                        return bytes;
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.Error($"[DownBytes]_:{ex}");
                }
            }
            return default(byte[]);
        }
    }


}
