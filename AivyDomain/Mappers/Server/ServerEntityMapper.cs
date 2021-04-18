﻿using AivyData.Entities;
using AivyDomain.API;
using Org.Mentalis.Network.ProxySocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AivyDomain.Mappers.Server
{
    public class ServerEntityMapper : IMapper<Func<ServerEntity, bool>, ServerEntity>
    {
        private readonly List<ServerEntity> _servers;

        public ServerEntityMapper()
        {
            _servers = new List<ServerEntity>();
        }

        public ServerEntity MapFrom(Func<ServerEntity, bool> input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            if(input(new ServerEntity()))
            {
                ServerEntity entity = new ServerEntity() 
                {
                    Socket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                    IsRunning = false
                };
                _servers.Add(entity);
                return entity;
            }

            return _servers.FirstOrDefault(input);
        }

        public bool Remove(Func<ServerEntity, bool> predicat)
        {
            return _servers.Remove(_servers.FirstOrDefault(predicat));
        }
    }
}
