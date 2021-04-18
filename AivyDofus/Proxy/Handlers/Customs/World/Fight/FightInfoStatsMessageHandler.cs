using AivyData.Model;
using AivyData.Model.Fights;
using AivyDofus.Handler;
using AivyDofus.Model.Fights;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Fight
{
    [ProxyHandler(ProtocolName = "GTM")]
    public class FightInfoStatsMessageHandler : AbstractMessageHandler
    {
        public FightInfoStatsMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            var account = _account;
            string[] separador = _package.Substring(4).Split('|');

            for (int i = 0; i < separador.Length; ++i)
            {
                string[] _loc6_ = separador[i].Split(';');
                int id = int.Parse(_loc6_[0]);
                AbstractFighter fighter = account.Character.Fight.GetFighterById(id);

                if (_loc6_.Length != 0)
                {

                    int healt_current = int.Parse(_loc6_[2]);
                    byte pa = byte.Parse(_loc6_[3]);
                    byte pm = byte.Parse(_loc6_[4]);
                    short cell = short.Parse(_loc6_[5]);
                    int healt_max = int.Parse(_loc6_[7]);

                    if (cell > 0)//son espectadores
                    {
                        byte equipo = Convert.ToByte(id > 0 ? 0 : 1);
                        if (fighter != null)
                        {
                            account.Character.Fight.UpdateFighter(fighter, healt_current, _loc6_[1].Equals("0"), pa, pm, healt_max, cell);
                        }
                        else
                        {
                            var newFighter = new MonsterFighter(id, equipo, _loc6_[1].Equals("0"), healt_current, healt_max, account.Character.Fight.Map.GetCellFromId(cell), pa, pm);
                            account.Character.Fight.AddFighter(newFighter);
                        }
                    }
                }
            }
        }
    }
}
