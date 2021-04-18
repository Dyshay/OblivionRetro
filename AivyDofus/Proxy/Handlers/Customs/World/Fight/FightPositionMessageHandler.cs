using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using DeepBot.Model.Account.Game.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Fight
{
    [ProxyHandler(ProtocolName ="GP")]
    public class FightPositionMessageHandler : AbstractMessageHandler
    {
        public FightPositionMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            Account account = _account;

            var map = account.Character.Map;

            account.Character.Fight.StartingCells.Clear();

            string[] _loc3 = _package.Substring(2).Split('|');

            for (int a = 0; a < _loc3[0].Length; a+=2)
            {
                account.Character.Fight.StartingCells.Add(map.GetCellFromId((short)((Hash.get_Hash(_loc3[0][a]) << 6) + Hash.get_Hash(_loc3[0][a + 1]))));
            }
        }
    }
}
