using AivyData.Model;
using AivyData.Model.Fights;
using AivyDofus.Handler;
using AivyDofus.Model.Maps;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Entities;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Action
{
    [ProxyHandler(ProtocolName = "GA")]
    public class GameActionMessageHandler : AbstractMessageHandler
    {
        public GameActionMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            string[] separador = _package.Substring(2).Split(';');
            if (separador.Length > 1)
            {
                int id_accion = int.Parse(separador[1]);
                var personaje = _account.Character;
                if (id_accion > 0)
                {
                    int id_entidad = 0;
                    if (separador.Length > 2)
                    {
                        id_entidad = int.Parse(separador[2]);
                    }
                    byte tipo_gkk_movimiento;
                    Cell celda;
                    AbstractFighter luchador;
                    var mapa = _account.Character.Map;
                    var pelea = _account.Character.Fight;

                    switch (id_accion)
                    {

                        case 1:
                            celda = mapa.GetCellFromId(Hash.Get_Cell_From_Hash(separador[3].Substring(separador[3].Length - 2)));

                            if (!_account.IsFighting())
                            {
                                if (id_entidad == personaje.Id && celda.CellId > 0 && personaje.Cell.CellId != celda.CellId)
                                {
                                    tipo_gkk_movimiento = byte.Parse(separador[0]);
                                    personaje.Cell = celda;

                                    //await cuenta.game.manager.movimientos.evento_Movimiento_Finalizado(celda, tipo_gkk_movimiento, true);
                                }
                                else if (mapa.Entities.TryGetValue(id_entidad, out Entity entidad))
                                {
                                    entidad.Cell = celda;

                                    //if (GlobalConfig.show_debug_messages)
                                    //    cuenta.Logger.LogInfo("DEBUG", "Mouvement détecté d'une entité vers la cellule : " + celda.cellId);
                                }
                                //mapa.GetEntitiesRefreshEvent();
                            }
                            else
                            {
                                luchador = pelea.GetFighterById(id_entidad);
                                if (luchador != null)
                                {
                                    luchador.Cell = celda;

                                    if (luchador.Id == personaje.Id)
                                    {
                                        tipo_gkk_movimiento = byte.Parse(separador[0]);

                                        await Task.Delay(400 + (100 * personaje.Cell.GetDistanceBetweenCells(celda)));
                                        _account.Send("GKK" + tipo_gkk_movimiento);
                                    }
                                }
                            }
                            break;

                        case 4:
                            separador = separador[3].Split(',');
                            celda = mapa.GetCellFromId(short.Parse(separador[1]));

                            //if (!cuenta.IsFighting() && id_entidad == personaje.id && celda.cellId > 0 && personaje.celda.cellId != celda.cellId)
                            //{
                            //    personaje.celda = celda;
                            //    await Task.Delay(150);
                            //    cuenta.connexion.SendPacket("GKK1");
                            //    mapa.GetEntitiesRefreshEvent();
                            //    cuenta.game.manager.movimientos.movimiento_Actualizado(true);
                            //}
                            break;

                        case 5:
                            if (_account.IsFighting())
                            {
                                separador = separador[3].Split(',');
                                luchador = pelea.GetFighterById(int.Parse(separador[0]));

                                if (luchador != null)
                                    luchador.Cell = mapa.GetCellFromId(short.Parse(separador[1]));
                            }
                            break;

                        case 102:
                            if (_account.IsFighting())
                            {
                                luchador = pelea.GetFighterById(id_entidad);
                                byte pa_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                                if (luchador != null)
                                    luchador.Pa -= pa_utilizados;
                            }
                            break;

                        case 103:
                            if (_account.IsFighting())
                            {
                                int id_muerto = int.Parse(separador[3]);

                                luchador = pelea.GetFighterById(id_muerto);
                                if (luchador != null)
                                    luchador.IsAlive = false;
                            }
                            break;

                        case 129: //movimiento en pelea con exito
                            if (_account.IsFighting())
                            {
                                luchador = pelea.GetFighterById(id_entidad);
                                byte pm_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                                if (luchador != null)
                                    luchador.Pm -= pm_utilizados;

                                //if (luchador.Id == personaje.Id)
                                //pelea.get_Movimiento_Exito(true);
                            }
                            break;

                        case 151://obstaculos invisibles
                            if (_account.IsFighting())
                            {
                                luchador = pelea.GetFighterById(id_entidad);

                                if (luchador != null && luchador.Id == personaje.Id)
                                {
                                    //cuenta.Logger.LogError("INFORMATION", "Il n'est pas possible d'effectuer cette action à cause d'un obstacle invisible.");
                                    //pelea.get_Hechizo_Lanzado(short.Parse(separador[3]), false);
                                }
                            }
                            break;

                        case 181: //efecto de invocacion (pelea)
                            celda = mapa.GetCellFromId(short.Parse(separador[3].Substring(1)));
                            short id_luchador = short.Parse(separador[6]);
                            short vida = short.Parse(separador[15]);
                            byte pa = byte.Parse(separador[16]);
                            byte pm = byte.Parse(separador[17]);
                            byte equipo = byte.Parse(separador[25]);

                            pelea.AddFighter(new AivyDofus.Model.Fights.MonsterFighter(id_luchador, equipo, true, vida, vida, celda, pa, pm, equipo, true));
                            break;

                            //case 302://fallo critico
                            //    if (cuenta.IsFighting() && id_entidad )
                            //        pelea.get_Hechizo_Lanzado(0, false);
                            //    break;

                            //case 300: //hechizo lanzado con exito
                            //    if (cuenta.IsFighting() && id_entidad == cuenta.game.character.id)
                            //    {
                            //        short celda_id_lanzado = short.Parse(separador[3].Split(',')[1]);
                            //        pelea.get_Hechizo_Lanzado(celda_id_lanzado, true);
                            //    }
                            //    break;

                            //case 501:
                            //    int tiempo_recoleccion = int.Parse(separador[3].Split(',')[1]);
                            //    celda = mapa.GetCellFromId(short.Parse(separador[3].Split(',')[0]));
                            //    byte tipo_gkk_recoleccion = byte.Parse(separador[0]);

                            //    await cuenta.game.manager.recoleccion.evento_Recoleccion_Iniciada(id_entidad, tiempo_recoleccion, celda.cellId, tipo_gkk_recoleccion);
                            //    break;

                            //case 900:
                            //    cuenta.connexion.SendPacket("GA902" + id_entidad, true);
                            //    cuenta.Logger.LogInfo("INFORMATION", "Le défi avec le personnage ID : " + id_entidad + " est annulée");
                            //    break;
                    }
                }

            }
        }
    }
}
