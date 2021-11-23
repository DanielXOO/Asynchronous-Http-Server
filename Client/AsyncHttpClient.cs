using System.Text.RegularExpressions;

namespace httpClient.DanielXOO
{
    class AsyncHttpClient
    {
        private readonly HttpClient _httpClient;
        public readonly Uri Url;

        public AsyncHttpClient(string url)
        {
            var checkURL = new Regex(@"https?:\/\/\w+(.\w)+?:\d{4}\/");
            var checkIP = new Regex(@"https?:\/\/(\d{1,3}.){3}(\d{1,3}):\d{4}\/");
            if (!checkURL.IsMatch(url) && !checkIP.IsMatch(url))
            {
                //TODO: Log error
                Console.WriteLine("Error");
                Environment.Exit(1);
            }
            Url = new Uri(url);
            _httpClient = new HttpClient();
        }
        
        public async Task SendAsync(string data,HttpMethod method)
        {
            var request = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = Url,
                Content = new StringContent(data),
            };
            var responce = await _httpClient.SendAsync(request);
            await Console.Out.WriteLineAsync(responce.Content.ReadAsStringAsync().Result);
        }

    }
}
