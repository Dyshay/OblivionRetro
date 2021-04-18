﻿using AivyData.API;
using AivyData.Entities;
using AivyDomain.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AivyDomain.UseCases.Client
{
    public class ClientCreatorRequest : IRequestHandler<IPEndPoint, ClientEntity>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<ClientEntity, ClientData> _repository;

        public ClientSenderRequest _client;

        public void UpdateClientSender(ClientSenderRequest client)
        {
            _client = client;
        }

        public ClientCreatorRequest(IRepository<ClientEntity, ClientData> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ClientEntity Handle(IPEndPoint request)
        {
            return _repository.ActionResult(x => true, x => 
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                if (x.IsRunning) throw new ArgumentException("client is already created");

                x.RemoteIp = request;

                return x;
            });
        }
    }
}
