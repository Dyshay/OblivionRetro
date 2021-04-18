using AivyData.Model;
using AivyData.Model.Scripts.Actions;
using AivyData.Model.Scripts.Managers;
using AivyDofus.Model.Scripts.Tags;
using AivyDofus.Protocol;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Scripts.Actions
{
    public class ChangeMapAction : IAction
    {
        private int cellId;
        public ChangeMapAction(ITag tag)
        {
            if (tag is ChangeMapTag)
                cellId = (tag as ChangeMapTag).CellID;
        }
        public async Task ProcessAction(Account account, LuaScriptManager LuaManger)
        {
            var path = PathFinder.Instance.GetPath(account.Character.Map, account.Character.Cell.CellId, cellId, false);
            account.Character.Cell = Map.Maps[account.Character.Map.MapId].Cells[cellId];
            var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
            account.Send($"GA001{pathString}");
            await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(account.Character.Map, path));
        }
    }
}
