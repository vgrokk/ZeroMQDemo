using System;
using System.Threading;
using fszmq;

namespace ZeroMQRunner.Demo3
{
    public class Ventilator : Endpoint, IDemo3Endpoint
    {
        public void Execute(Demo3Input input)
        {
            using (var context = new Context())
            using (var sender = context.Push())
            {
                sender.Bind("tcp://*:5557");

                Console.Write("Press enter when the workers are ready: ");
                Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine("Sending tasks to workers...");

                // Signal the start of the batch
                sender.Send(BitConverter.GetBytes(0));

                var random = new Random();
                for (var i = 0; i < 300; i++)
                {
                    var sleepTime = random.Next(1, 100);
                    var message = BitConverter.GetBytes(sleepTime);
                    sender.Send(message);
                    Thread.Sleep(3000);

                }
            }
        }

        public void PositionWindow(Demo3Input input)
        {
            Console.SetWindowSize(45, 10);
            Console.SetBufferSize(45, 10);
            ConsoleApp.MoveWindow(25, 100);
        }
    }
}