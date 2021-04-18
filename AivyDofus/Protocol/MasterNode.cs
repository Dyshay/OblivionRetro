using AivyData.Entities;
using AivyData.Model;
using AivyDomain.UseCases.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Protocol
{
    public class MasterNode
    {
        public ClientSenderRequest Client;
        public ProxyEntity Proxy;
        public Account Account;
        public ClientEntity ClientDofus;
        public ClientEntity Remote;

        public MasterNode(string username, string password)
        {
            Account = new Account()
            {
                Username = username,
                Password = password
            };
        }
    }
}
