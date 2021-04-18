using AivyData.Model;
using AivyData.Model.Fights;
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
    [ProxyHandler(ProtocolName = "GIC")]
    public class FightChangePositionMessageHandler : AbstractMessageHandler
    {
        public FightChangePositionMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            string[] separador_posiciones = _package.Substring(4).Split('|');
            int id_entidad;
            short celda;
            var mapa = _account.Character.Map;

            foreach (string posicion in separador_posiciones)
            {
                id_entidad = int.Parse(posicion.Split(';')[0]);
                celda = short.Parse(posicion.Split(';')[1]);

                if (id_entidad == _account.Character.Id)
                {
                    if (_account.Character.IsAutoClick)
                    {
                        _account.Send("GR1");//boton listo
                    }
                }

                AbstractFighter fighter = _account.Character.Fight.GetFighterById(id_entidad);
                if (fighter != null)
                {
                    _account.Character.Fight.UpdateCellFighter(fighter, celda);
                }

            }
        }
    }
}
