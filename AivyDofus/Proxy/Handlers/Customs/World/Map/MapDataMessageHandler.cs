using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using Mapping = DeepBot.Model.Account.Game.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AivyData.Model;
using DeepBot.Utilities.Pathfinding;

namespace AivyDofus.Proxy.Handlers.Customs.World.Map
{
    [ProxyHandler(ProtocolName = "GDM")]
    public class MapDataMessageHandler : AbstractMessageHandler
    {
        public MapDataMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {

            string[] _loc3 = _package.Substring(4).Split('|');
            if (_package.Substring(4).Length == 21)
            {
                // Required in Amakna;
                _account.Character.OnChangeMap(Mapping.Map.Maps[int.Parse(_loc3[1])]);
            }
            else
            {
                _account.Character.OnChangeMap(Mapping.Map.Maps[int.Parse(_loc3[0])]);
                if (_account.Character.IsAutoClick)
                {
                    var groups = _account.Character.GetGroupsMonster(1, 500);

                    foreach (var group in groups)
                    {
                        var path = PathFinder.Instance.GetPath(_account.Character.Map, _account.Character.Cell.CellId, group.Cell.CellId, true);
                        var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
                        _account.Send($"GA001{pathString}");
                        await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(_account.Character.Map, path));
                    }
                }
            }
        }
    }
}
