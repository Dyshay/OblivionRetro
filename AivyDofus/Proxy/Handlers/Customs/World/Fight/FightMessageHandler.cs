using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDofus.Proxy.Callbacks;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Fight
{
    [ProxyHandler(ProtocolName = "GTS")]
    public class FightTurnStartMessageHandler : AbstractMessageHandler
    {
        public FightTurnStartMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => false;

        public override async Task Handle()
        {
            //if(_pack)
            var Character = _account.Character;
            if (int.Parse(_package.Substring(3).Split('|')[0]) != Character.Id)
                return;
            await Task.Delay(400);
            //await Character.Fight.onTurnStart();
        }

    }
}
