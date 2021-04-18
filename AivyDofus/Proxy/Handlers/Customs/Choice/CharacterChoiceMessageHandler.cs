using AivyData.Enums;
using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.Choice
{
    [ProxyHandler(ProtocolName = "ASK")]
    public class CharacterChoiceMessageHandler : AbstractMessageHandler
    {
        public CharacterChoiceMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            string[] _loc4 = _package.Substring(4).Split('|');

            int id = int.Parse(_loc4[0]);
            string name = _loc4[1];
            byte level = byte.Parse(_loc4[2]);
            short breedId = short.Parse(_loc4[3]);
            short sex = short.Parse(_loc4[4]);

            _account.Character.Nickname = name;
            _account.Character.Id = id;
            _account.Character.Level = level;
            _account.Character.BreedId = breedId;
            _account.Character.Sex = sex;
            _account.Character.Inventory.getInventory(_loc4[9]);


            //cuenta.game.character.evento_Personaje_Seleccionado();
            //account.game.character.timer_afk.Change(1200000, 1200000);
            _account.State = AccountState.IDLE;
        }
    }
}
