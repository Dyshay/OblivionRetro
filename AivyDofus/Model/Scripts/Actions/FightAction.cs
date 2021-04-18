using AivyData.Model.Scripts.Managers;
using AivyDofus.Protocol;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AivyData.Model.Scripts.Actions
{
    public class FightAction : IAction
    {
        public async Task ProcessAction(Account account, LuaScriptManager luaScriptManager)
        {
            int monsters_max = luaScriptManager.Get("MONSTERS_MAX", MoonSharp.Interpreter.DataType.Number, 8);
            int monsters_min = luaScriptManager.Get("MONSTERS_MIN", MoonSharp.Interpreter.DataType.Number, 1);
            int monsters_level_min = luaScriptManager.Get("MONSTERS_LEVEL_MIN", MoonSharp.Interpreter.DataType.Number, 1);
            int monsters_level_max = luaScriptManager.Get("MONSTERS_LEVEL_MAX", MoonSharp.Interpreter.DataType.Number, 31);

            var groups = account.Character.GetGroupsMonster(monsters_level_min, monsters_level_max);

            foreach (var group in groups)
            {
                var path = PathFinder.Instance.GetPath(account.Character.Map, account.Character.Cell.CellId, group.Cell.CellId, true);
                var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
                account.Send($"GA001{pathString}");
                await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(account.Character.Map, path));
            }
        }
    }
}
