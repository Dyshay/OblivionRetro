using AivyData.Enums;
using AivyData.Model;
using AivyData.Model.Fights;
using AivyDofus.Handler;
using AivyDofus.Model.Fights;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Entities;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Map
{
    [ProxyHandler(ProtocolName = "GM")]
    public class MouvementMessageHandler : AbstractMessageHandler
    {
        public MouvementMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {

        }
        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
        }
        //    var Account = _account;
        //    string[] playersSplit = _package.Substring(3).Split('|'), infos;
        //    string _loc6, templateNumber, type;

        //    for (int i = 0; i < playersSplit.Length; ++i)
        //    {
        //        _loc6 = playersSplit[i];
        //        if (_loc6.Length != 0)
        //        {
        //            infos = _loc6.Substring(1).Split(';');
        //            if (_loc6[0].Equals('+'))
        //            {
        //                Cell cell = Account.Character.Map.Cells.FirstOrDefault(cellule => cellule.CellId == short.Parse(infos[0]));
        //                //Pelea fight = account.Game.fight;
        //                int id = int.Parse(infos[3]);
        //                templateNumber = infos[4];
        //                type = infos[5];
        //                if (type.Contains(","))
        //                    type = type.Split(',')[0];

        //                switch (int.Parse(type))
        //                {
        //                    case -1: // Fighter pop
        //                    case -2:
        //                        if (Account.State == AccountState.FIGHTING)
        //                        {
        //                            int health = int.Parse(infos[12]);
        //                            byte pa = byte.Parse(infos[13]);
        //                            byte pm = byte.Parse(infos[14]);
        //                            byte team = byte.Parse(infos[15]);

        //                            Account.Character.Fight.AddFighter(new MonsterFighter(id, team, true, health, health, cell, pa, pm, 0));
        //                        }
        //                        break;
        //                    case -3: // Monsters pop
        //                        string[] templates = templateNumber.Split(',');
        //                        string[] levels = infos[7].Split(',');

        //                        Monster monster = new Monster()
        //                        {
        //                            Id = id,
        //                            Template = int.Parse(templates[0]),
        //                            Cell = cell,
        //                            Level = int.Parse(levels[0]),
        //                        };
        //                        for (int m = 1; m < templates.Length; ++m)
        //                            monster.Group.Add(new Monster()
        //                            {
        //                                Id = id,
        //                                Template = int.Parse(templates[m]),
        //                                Cell = cell,
        //                                Level = int.Parse(levels[m]),
        //                            });
        //                        Account.Character.Map.Entities.TryAdd(id, monster);
        //                        if (Account.Character.IsAutoClick)
        //                        {
        //                            var path = PathFinder.Instance.GetPath(Account.Character.Map, Account.Character.Cell.CellId, cell.CellId, true);
        //                            var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
        //                            Account.Send($"GA001{pathString}");
        //                            await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(Account.Character.Map, path));
        //                        }
        //                        break;
        //                    case -4: // NPC pop
        //                        Account.Character.Map.Entities.TryAdd(id, new Npc()
        //                        {
        //                            Id = id,
        //                            Cell = cell,
        //                            TemplateId = int.Parse(templateNumber),
        //                        });
        //                        break;

        //                    case -5:
        //                    case -6:
        //                    case -7:
        //                    case -8:
        //                    case -9:
        //                    case -10:
        //                        break;

        //                    default: // Player pop
        //                        if (Account.State != AccountState.FIGHTING)
        //                        {
        //                            if (Account.Character.Id != id) // Add Players
        //                            {
        //                                Account.Character.Map.Entities.TryAdd(id, new Player()
        //                                {
        //                                    Id = id,
        //                                    Cell = cell,
        //                                    Name = templateNumber,
        //                                    Sex = byte.Parse(infos[7]),
        //                                });
        //                            }
        //                            else // Add own Character
        //                            {
        //                                Account.Character.Cell = cell;
        //                                Account.Character.Map.Entities.Clear();
        //                                Account.Character.Map.Entities.TryAdd(id, new DeepBot.Model.Account.Game.Maps.Entities.Character()
        //                                {
        //                                    Id = id,
        //                                    Cell = cell,
        //                                    Name = templateNumber,
        //                                    Sex = byte.Parse(infos[7]),
        //                                });
        //                            }

        //                        }
        //                        else
        //                        {
        //                            int health = int.Parse(infos[14]);
        //                            byte pa = byte.Parse(infos[15]);
        //                            byte pm = byte.Parse(infos[16]);
        //                            byte team = byte.Parse(infos[24]);


        //                            if (Account.Character.Id == id)
        //                            {
        //                                short cell_position = Account.Character.Fight.GetCellNearOrFar(false, Account.Character.Fight.StartingCells);
        //                                /** la posicion es aleatoria pero el paquete GP siempre aparecera primero el team donde esta el pj **/

        //                                if (cell_position != cell.CellId)
        //                                {
        //                                    await Task.Run(async () =>
        //                                    {
        //                                        //await Task.Delay(680);
        //                                        //Account.Send("Gp" + cell_position);
        //                                        Account.Character.Fight.AddFighter(new CharacterFighter(id, team, true, health, health, Account.Character.Map.Cells[cell_position], pa, pm, Account.Character.Characteristics, Account.Character.Spells));
        //                                        //await Task.Delay(680);
        //                                        //Account.Send("GR1");
        //                                    });
        //                                }
        //                                //cuenta.connexion.SendPacket("Gp" + celda_posicion, true);
        //                                else
        //                                {
        //                                    Account.Character.Fight.AddFighter(new CharacterFighter(id, team, true, health, health, Account.Character.Map.Cells[cell_position], pa, pm, Account.Character.Characteristics, Account.Character.Spells));
        //                                    //Account.Send("GR1");
        //                                }

        //                            }
        //                        }
        //                        break;
        //                }
        //            }
        //            else if (_loc6[0].Equals('-'))
        //            {
        //                if (Account.State != AccountState.FIGHTING)
        //                {
        //                    int id = int.Parse(_loc6.Substring(1));
        //                    Account.Character.Map.Entities.TryRemove(id, out Entity entity);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
