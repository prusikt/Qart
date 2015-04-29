﻿using Qart.Core.Io.FileRolling;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qart.FileForwarder
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "localhost";
            string queueName = "";

            var publisher = new RabbitMqPublisher("localhost", "queueName", "exchange", "routing");

            var manager = new RollingFileReaderManager(@"C:\Work\Projects\QartStage\Src\Qart.RandomLogger\bin\Debug\log.txt",
                                                        new FileBasedPositionStore(@"c:\work\output"),
                                                        ReadBehaviour.FromWhereLeft,
                                                        new DummyOutputProvider(@"c:\work\output"));


            Console.ReadKey();
        }
    }
}