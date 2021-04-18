using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Model.Characters.Inventorys;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Character
{
    [ProxyHandler(ProtocolName ="EmKO+")]
    public class CharacterForgeAddItemMessageHandler : AbstractMessageHandler
    {
        public CharacterForgeAddItemMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            if (_account.Character.Forge.IsStart)
            {
                _account.Character.Forge.IsChangeItems = true;
                var guidPackage = _package.Substring(5).Split('|')[0];
                _account.Character.Inventory.Items.TryGetValue(int.Parse(guidPackage), out Item item);
                _account.Character.Forge.CurrentItem = item;
                _account.Character.Forge.IsChangeItems = false;
            }
        }
    }
}
