using System;
using System.Net;

namespace httpServer.DanielXOO
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new AsyncHttpServer("http://26.31.165.3:8080/");
            server.Start().Wait(); 
        }
    }
}
