﻿using System;
using System.Text;
using fszmq;

namespace ZeroMQRunner.Demo2
{
    public class Client : Endpoint, IDemo2Endpoint
    {
        public void Execute(Demo2Input input)
        {
            using (var context = new Context())
            using (var subscriber = context.Sub())
            {
                AddSubscription(subscriber, input.ZipCodeFlag);
                subscriber.Connect("tcp://localhost:5556");

                ListenForEvents(subscriber);
            }
        }

        private void AddSubscription(Socket subscriber, string zipCode)
        {
            if (zipCode == "0")
                zipCode = string.Empty;
            var topic = Encoding.Unicode.GetBytes(zipCode);
            subscriber.Subscribe(new[] { topic });
        }

        private static void ListenForEvents(Socket subscriber)
        {
            while (true)
            {
                var message = subscriber.Recv();
                var values = Encoding.Unicode.GetString(message).Split('|');
                Console.WriteLine("ZipCode: {0}, Temp: {1}, Humidity: {2}", values[0], values[1], values[2]);
            }
        }

        public void PositionWindow(Demo2Input input)
        {
            Console.SetWindowSize(45, 20);
            Console.SetBufferSize(45, 20);

            var clientNumber = int.Parse(input.ClientIdFlag);
            ConsoleApp.MoveWindow(450 + (25 * (clientNumber - 1)), 100 + (200 * (clientNumber - 1)));
        }
    }
}