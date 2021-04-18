using AivyData.Entities;
using AivyData.Model;
using AivyDofus.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Protocol
{
    public class ProxyManager
    {
        public Dictionary<int, MasterNode> Clients;
        private int PortCount { get; set; } = 666;
        private static ProxyManager _Instance;
        public static ProxyManager Instance
        {
            get { return _Instance ?? (_Instance = new ProxyManager()); }
        }

        public void AddClient(string username, string password)
        {
            if (Instance.Clients == null)
                Instance.Clients = new Dictionary<int, MasterNode>();

            //DofusRetroProxy proxy = new DofusRetroProxy(@"C:\Users\ABCD\AppData\Local\Ankama\zaap\retro\resources\app\retroclient");
            //ProxyEntity entity = proxy.Active(true, PortCount);
            //Instance.Clients.Add(PortCount, new MasterNode(username, password));
            Instance.PortCount++;
        }

        public Account GetAccount(int port)
        {
            if (Instance.Clients.TryGetValue(port, out MasterNode value))
                return value.Account;
            return null;
        }

        public async Task Send(int port, string package, bool isClient = false)
        {
            //Clients[port].Client.Handle(Clients[port].ClientDofus, Encoding.UTF8.GetBytes(package));
            if (!isClient)
            {
                Clients[port].Client.Handle(Clients[port].Remote, Encoding.UTF8.GetBytes(package + "\n"));
            }
            else
                Clients[port].Client.Handle(Clients[port].ClientDofus, Encoding.UTF8.GetBytes(package + "\n"));
        }
    }
}
