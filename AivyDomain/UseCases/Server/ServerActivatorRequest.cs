using AivyData.API;
using AivyData.Entities;
using AivyDomain.Callback.Client;
using AivyDomain.Callback.Server;
using AivyDomain.Repository;
using NLog;
using Org.Mentalis.Network.ProxySocket;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AivyDomain.UseCases.Server
{
    public class ServerActivatorRequest : IRequestHandler<ServerEntity, bool, ServerAcceptCallback, ServerEntity>
    {
        private readonly IRepository<ServerEntity, ServerData> _repository;

        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ServerActivatorRequest(IRepository<ServerEntity, ServerData> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ServerEntity Handle(ServerEntity request1, bool request2, ServerAcceptCallback request3)
        {
            return _repository.ActionResult(x => x.Port == request1.Port, x => 
            {
                if (x.IsRunning == request2)
                    throw new Exception($"server running state is already : {x.IsRunning}");

                if (request2)
                {
                    ProxySocket Socket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Socket.ProxyEndPoint = new IPEndPoint(IPAddress.Parse("109.248.110.177"), 24532);
                    Socket.ProxyUser = "nemulg";
                    Socket.ProxyPass = "Dz4RNr2Ipk";
                    Socket.ProxyType = ProxyTypes.Socks5;
                    x.Socket = Socket;
                    x.Socket.Bind(new IPEndPoint(IPAddress.Any, x.Port));
                    x.Socket.Listen(10);

                    x.Socket.BeginAccept(request3.Callback, x.Socket);
                }
                else
                {
                    x.Socket.Dispose();
                }

                x.IsRunning = request2;

                logger.Info($"server running state : {x.IsRunning}");

                return x;
            });
        }
    }
}
