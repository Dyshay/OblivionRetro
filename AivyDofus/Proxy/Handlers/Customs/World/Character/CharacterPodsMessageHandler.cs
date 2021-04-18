using AivyData.Model;
using AivyDofus.Handler;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Character
{
    [ProxyHandler(ProtocolName = "Ow")]
    public class CharacterPodsMessageHandler : AbstractMessageHandler
    {
        public CharacterPodsMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            string[] pods = _package.Substring(2).Split('|');
            short pods_actual = short.Parse(pods[0]);
            short pods_max = short.Parse(pods[1]);
            var personaje = _account.Character;

            personaje.ActualPods = pods_actual;
            personaje.MaxPods = pods_max;
            _account.AccountStateUpdate?.Invoke();
        }
    }
}
