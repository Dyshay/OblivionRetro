using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Fight
{
    [ProxyHandler(ProtocolName = "GE")]
    public class FightEndMessageHandler : AbstractMessageHandler
    {
        public FightEndMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            _account.Character.Fight.onFightEnd();
            _account.Character.IsAutoClick = false;
        }
    }
}
