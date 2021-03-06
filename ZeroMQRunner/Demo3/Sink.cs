﻿using System;
using System.Diagnostics;
using System.Text;
using fszmq;

namespace ZeroMQRunner.Demo3
{
    public class Sink : Endpoint, IDemo3Endpoint
    {
        public void Execute(Demo3Input input)
        {
            using (var context = new Context())
            using (var receiver = context.Pull())
            {
                receiver.Bind("tcp://*:5558");

                // Assume we are waiting for 300 workitems
                var message = receiver.Recv();
                var stopwatch = Stopwatch.StartNew();
                for (var i = 0; i < 300; i++)
                {
                    var workerId = Encoding.Unicode.GetString(message);
                    Console.WriteLine("Worker {0} has finished a workitem", workerId);
                    message = receiver.Recv();
                }

                stopwatch.Stop();
                Console.WriteLine("Total elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
            }
        }

        public void PositionWindow(Demo3Input input)
        {
            Console.SetWindowSize(45, 15);
            Console.SetBufferSize(45, 15);
            ConsoleApp.MoveWindow(25, 300);
        }
    }
}