using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Model.Scripts.Actions;
using AivyDomain.Callback.Client;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = DeepBot.Model.Account.Game.Maps;

namespace AivyDofus.Proxy.Handlers.Customs.World.Interactives
{
    [ProxyHandler(ProtocolName = "GDF")]
    public class InteractiveStateMessageHandler : AbstractMessageHandler
    {
        public InteractiveStateMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            string interac = _package.Substring(4).Split('|').LastOrDefault();
            int lastCellid = 0;
            if (!string.IsNullOrEmpty(interac))
            {
                string[] separador2 = interac.Split(';');
                if (separador2.Length < 2)
                    return;
                lastCellid = int.Parse(separador2[0]);
            }

            foreach (string interactivo in _package.Substring(4).Split('|'))
            {
                string[] separador = interactivo.Split(';');
                if (separador.Length < 2)
                    return;
                Account cuenta = _account;
                short celda_id = short.Parse(separador[0]);
                byte estado = byte.Parse(separador[1]);

                switch (estado)
                {
                    case 2:
                        cuenta.Character.Map.Interactives[celda_id].IsUsable = false;
                        break;

                    case 3:
                        if (cuenta.Character.Map.Interactives.TryGetValue(celda_id, out var value))
                            value.IsUsable = false;

                        //if (cuenta.IsGathering())
                        //    cuenta.game.manager.recoleccion.evento_Recoleccion_Acabada(RecoleccionResultado.RECOLECTADO, celda_id);
                        //else
                        //    cuenta.game.manager.recoleccion.evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, celda_id);
                        break;

                    case 4:// reaparece asi se fuerza el cambio de mapa 
                        cuenta.Character.Map.Interactives[celda_id].IsUsable = false;
                        if (celda_id == lastCellid)
                        {
                            foreach (var item in cuenta.Character.Map.Interactives)
                            {
                                if (item.Value.IsUsable)
                                {
                                    cuenta.ScriptManager.AddAction(new GatheringAction(item.Key));
                                }
                            }
                        }
                        break;
                    case 5:
                        if (cuenta.Character.Map.Interactives.TryGetValue(celda_id, out var data))
                            data.IsUsable = true;
                        cuenta.ScriptManager.AddAction(new GatheringAction(celda_id));
                        //    var path = PathFinder.Instance.GetPath(_account.Character.Map, _account.Character.Cell.CellId, celda_id, false);
                        //_account.Character.Cell = C.Map.Maps[_account.Character.Map.MapId].Cells[celda_id];
                        //var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
                        //_account.Send($"GA001{pathString}");
                        //await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(_account.Character.Map, path));
                        //cuenta.Send("GA500" + celda_id + ";" + 24);
                        break;
                }
            }
        }
    }
}
