using System;
using System.Configuration;
using System.Net;
using System.Threading;
using SimpleTCP;

namespace FakeElasticCache
{
    internal class Listener
    {
        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

        public static void Main()
        {

            var memcachedPort = int.Parse(ConfigurationManager.AppSettings["MemcachedPort"]);
            var memcachedVersion = ConfigurationManager.AppSettings["MemcachedVersion"];
            var memcachedIp = IPAddress.Parse(ConfigurationManager.AppSettings["MemcachedAddress"]);
            var serverInfo = $"1\nmemcached|{memcachedIp}|{memcachedPort}";
            var listenPort = int.Parse(ConfigurationManager.AppSettings["ListenPort"]);

            Console.CancelKeyPress += OnCancelKeyPress;

            Console.WriteLine($"Memcached server version: {memcachedVersion}, ip: {memcachedIp}, port: {memcachedPort}" );
            Console.WriteLine($"Starting Elastic Cache Listener on Port {listenPort}...");

            var server = new SimpleTcpServer()
                .Start(listenPort);

            Console.WriteLine("Press ctrl+c to end.");

            server.DataReceived += (sender, msg) =>
            {
                string response;

                Console.WriteLine("Request :" + msg.MessageString);

                var request = msg.MessageString.Trim();

                if (request.Equals("stats"))
                {
                    response = $"STAT version {memcachedVersion}\r\nEND\r\n";
                }
                else if (request.StartsWith("config") || request.StartsWith("gets"))
                {
                    response =
                        $"CONFIG cluster 0 {serverInfo.Length}\r\n{serverInfo}\r\nEND\r\n";
                }
                else
                {
                    response = $"Command {msg.MessageString} not supported";
                }

                Console.WriteLine("Response :" + response);

                msg.Reply(response);
            };

            AutoResetEvent.WaitOne();

            server.Stop();
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            AutoResetEvent.Set();
        }
    }
}