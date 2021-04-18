using AivyData.Model;
using AivyData.Model.Scripts.Actions;
using AivyData.Model.Scripts.Managers;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Interactives;
using DeepBot.Utilities.Pathfinding;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AivyDofus.Model.Scripts.Actions
{
    public class GatheringAction : IAction
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public int CellId { get; set; }
        public GatheringAction(int cellid)
        {
            CellId = cellid;
        }

        public async Task ProcessAction(Account account, LuaScriptManager LuaManger)
        {
            var path = PathFinder.Instance.GetPath(account.Character.Map, account.Character.Cell.CellId, CellId, true, account.Character.Map.Cells.Where(c => !c.IsWalkable).Select(c => (int)c.CellId).ToList());
            account.Character.Cell = Map.Maps[account.Character.Map.MapId].Cells[CellId];
            if (account.Character.Map.Interactives.TryGetValue(CellId, out DeepBot.Model.Account.Game.Maps.Interactives.InteractivObject obj) && obj.IsUsable)
            {
                var interactive = InteractivObject.InteractivesObjects[obj.Id];
                if (path != null)
                {
                    await Task.Run(async () =>
                    {
                        var skill = account.Character.Skills.Find(c => interactive.IdSkill.Contains(c.Id));
                        var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
                        account.Send($"GA001{pathString}");
                        account.State = AivyData.Enums.AccountState.WALKING;
                        await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(account.Character.Map, path));
                        account.Send("GA500" + CellId + ";" + interactive.IdSkill);
                        account.State = AivyData.Enums.AccountState.GATHERING;
                        await Task.Delay((int)skill.GatheringTime + 500);
                        account.State = AivyData.Enums.AccountState.IDLE;
                    });
                }
                else
                {
                    path = PathFinder.Instance.GetPath(account.Character.Map, account.Character.Cell.CellId, CellId, true);
                    if (path != null)
                    {
                        var skill = account.Character.Skills.Find(c => interactive.IdSkill.Contains(c.Id));
                        var pathString = PathFinderUtils.Instance.GetPathfindingString(path);
                        account.Send($"GA001{pathString}");
                        account.State = AivyData.Enums.AccountState.WALKING;
                        await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(account.Character.Map, path));
                        account.Send("GA500" + CellId + ";" + interactive.IdSkill);
                        account.State = AivyData.Enums.AccountState.GATHERING;
                        await Task.Delay((int)skill.GatheringTime);
                        account.State = AivyData.Enums.AccountState.IDLE;
                    }
                }

            }

        }
    }
}
