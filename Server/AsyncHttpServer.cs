using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace httpServer.DanielXOO
{
    class AsyncHttpServer
    {
        private readonly HttpListener _httpListener;
        private HttpListenerContext _context;
        public readonly Uri Url;
        public AsyncHttpServer(string url)
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
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(Url.ToString());
        }

        public async Task Start()
        {
            _httpListener.Start();
            await Console.Out.WriteLineAsync("Server started");
            while (true)
            {
                _context = await _httpListener.GetContextAsync();
                Console.WriteLine($"Client connected {_context.Request.RemoteEndPoint.Address}");
                WriteResponce("Message sent", 200);
                OutputRequest();
            }
        }

        public async void WriteResponce(string data, int status)
        {
            HttpListenerResponse response = _context.Response;
            response.StatusCode = status;
            response.ContentType = "application/json";
            response.Headers.Add($"Date: {DateTime.Now}");
            using (var stream = new StreamWriter(_context.Response.OutputStream))
            {
                await stream.WriteAsync(data);
                await stream.FlushAsync();
            }
        }

        public async void OutputRequest()
        {
            HttpListenerRequest request = _context.Request;
            foreach (string item in request.Headers)
            {
                //TODO: Log connections
                await Console.Out.WriteLineAsync($"{item} {request.Headers[item]}");
            }
            if (request.HasEntityBody)
            {
                using (var stream = request.InputStream)
                {
                    try
                    {
                        byte[] buffer = new byte[request.ContentLength64];
                        await stream.ReadAsync(buffer);
                        await Console.Out.WriteLineAsync(Encoding.Default.GetString(buffer));
                        await stream.FlushAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Data error");
                        //TODO: Log ex in file
                        Environment.Exit(1);
                    }
                }
            }
        }

        public async Task Stop()
        {
            await Console.Out.WriteLineAsync("Server stoped");
            if (_httpListener.IsListening)
            {
                _httpListener.Stop();
                _httpListener.Close();
            }
        }
    }
}
