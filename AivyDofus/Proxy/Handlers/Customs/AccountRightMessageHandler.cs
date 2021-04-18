using AivyData.Model;
using AivyDofus.Handler;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs
{
    [ProxyHandler(ProtocolName = "AlK")]
    public class AccountRightMessageHandler : AbstractMessageHandler
    {
        public AccountRightMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            _account.SendFromServer("AlK1");
        }
    }
}
