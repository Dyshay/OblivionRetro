using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Character
{
    [ProxyHandler(ProtocolName = "SL")]
    public class CharacterSpellsMessageHandler : AbstractMessageHandler
    {
        public CharacterSpellsMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            if (!_package[2].Equals('o'))
            {
               _account.Character.ActualizeSpellList(_package.Substring(2));
            }
        }
    }
}
