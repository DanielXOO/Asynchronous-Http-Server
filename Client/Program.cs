using System;

namespace httpClient.DanielXOO
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new AsyncHttpClient("http://26.31.165.3:8080/");
            client.SendAsync("Hola", HttpMethod.Post).Wait();
        }
    }
}