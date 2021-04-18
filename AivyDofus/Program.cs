﻿using AivyData.Entities;
using AivyDofus.Protocol;
using AivyDofus.Protocol.Parser;
using AivyDofus.Proxy;
using AivyDofus.Proxy.Callbacks;
using AivyDofus.Server;
using AivyDomain.API.Proxy;
using AivyDomain.Callback.Proxy;
using AivyDomain.Mappers.Proxy;
using AivyDomain.Repository.Proxy;
using AivyDomain.UseCases.Proxy;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AivyDofus
{
    public class Program
    {
        static readonly ConsoleTarget log_console = new ConsoleTarget("log_console");
        static readonly FileTarget log_file = new FileTarget("log_file") { FileName = "./log.txt" };
        static readonly LoggingConfiguration configuration = new LoggingConfiguration();

        public static void Main(string[] args)
        {
            Console.Title = "AivyCore - 1.0.0";

            configuration.AddRule(LogLevel.Debug, LogLevel.Fatal, log_console);
            LogManager.Configuration = configuration;

            DofusServer server = new DofusServer(@"C:\Users\ABCD\AppData\Local\Ankama\zaap\retro\resources\app\retroclient");
            ServerEntity s_entity = server.Active(true, 777);

            Console.ReadLine();
        }
    }
}
