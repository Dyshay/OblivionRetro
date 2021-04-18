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
    [ProxyHandler(ProtocolName = "GJK")]
    public class FightStartMessageHandler : AbstractMessageHandler
    {
        public FightStartMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            var Account = _account;

            string[] separador = _package.Substring(3).Split('|');
            byte estado_pelea = byte.Parse(separador[0]);

            switch (estado_pelea)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    await Account.Character.Fight.FighStart();
                    break;
            }
        }
    }
}
