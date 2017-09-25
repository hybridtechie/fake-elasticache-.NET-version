using System;
using System.Net;
using SimpleTCP;

namespace MyTcpListener
{
    internal class FakeElasticCacheListener
    {
        public static void Main()
        {
            const int memcachedPort = 11211;
            const string memcachedVersion = "1.4.14";
            var localAddr = IPAddress.Parse("127.0.0.1");
            var memcachedServer = $"1\nmemcached|{localAddr}|{memcachedPort}";
            var server = new SimpleTcpServer().Start(11212);
            Console.WriteLine("Starting Listener....");
            server.DataReceived += (sender, msg) =>
            {
                string response = null;
                Console.WriteLine("Request :" + msg.MessageString);
                var request = msg.MessageString.Trim();
                if (request.Equals("stats"))
                {
                    response = $"STAT version {memcachedVersion}\r\nEND\r\n";
                }
                else if (request.StartsWith("config") || request.StartsWith("gets"))
                {
                    response =
                        $"CONFIG cluster 0 {memcachedServer.Length}\r\n{memcachedServer}\r\nEND\r\n";
                }
                else
                {
                    response = $"Command {msg.MessageString} not supported";
                }
                Console.WriteLine("Response :" + response);
                msg.Reply(response);
            };

            while (true)
            {
                //wait
            }
        }
    }
}