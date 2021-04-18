using Org.Mentalis.Network.ProxySocket;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace AivyData.Entities
{
    public class ServerEntity
    {
        public ProxySocket Socket { get; set; }
        public bool IsRunning { get; set; } 
        public int Port { get; set; }
    }
}
