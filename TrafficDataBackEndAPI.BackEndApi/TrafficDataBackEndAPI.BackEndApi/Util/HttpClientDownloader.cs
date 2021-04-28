using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrafficDataBackEndAPI.BackEndApi.Util
{
    public class HttpClientDownloader
    {
        private readonly HttpClient client;

        public HttpClientDownloader(HttpClient client)
        {
            this.client = client;
        }

        public async Task<bool> DownloadFile(string url, string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                await using var ms = await response.Content.ReadAsStreamAsync();
                await using var fs = File.Create(fileInfo.FullName);
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(fs);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}
