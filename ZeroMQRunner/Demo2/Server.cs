using System;
using System.Text;
using System.Threading;
using fszmq;
using FubuCore;

namespace ZeroMQRunner.Demo2
{
    public class Server : Endpoint, IDemo2Endpoint
    {
        public void Execute(Demo2Input input)
        {
            using (var context = new Context())
            using (var publisher = context.Pub())
            {
                publisher.Bind("tcp://*:5556");
                PublishMessages(publisher);
            }
        }

        private static void PublishMessages(Socket publisher)
        {
            var random = new Random();
            while (true)
            {
                // Generate some accurate weather data
                var zip = random.Next(100000);
                var temp = random.Next(-80, 135);
                var humidity = random.Next(10, 100);

                // Generate frequent updates for 84101
                if (random.Next(0, 4) == 0)
                    zip = 84101;

                var message = "{0}|{1}|{2}".ToFormat(zip, temp, humidity);
                Console.WriteLine("Publishing: {0}", message);

                publisher.Send(Encoding.Unicode.GetBytes(message));
                Thread.Sleep(500);
            }
        }

        public void PositionWindow(Demo2Input input)
        {
            Console.SetWindowSize(45, 20);
            Console.SetBufferSize(45, 20);
            ConsoleApp.MoveWindow(25, 100);
        }
    }
}