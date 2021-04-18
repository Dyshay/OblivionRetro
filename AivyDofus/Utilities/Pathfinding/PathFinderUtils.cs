using AivyData.Enums.Map;
using AivyData.Utilities.Animation;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepBot.Utilities.Pathfinding
{
    public class PathFinderUtils
    {
        private static readonly Lazy<PathFinderUtils> lazy = new Lazy<PathFinderUtils>(() => new PathFinderUtils());

        public static PathFinderUtils Instance { get { return lazy.Value; } }

        private PathFinderUtils() { }

        private readonly Dictionary<AnimationType, AnimationDuration> AnimationTimes = new Dictionary<AnimationType, AnimationDuration>()
        {
            { AnimationType.DRAGO, new AnimationDuration(135, 200, 120) },
            { AnimationType.RUN, new AnimationDuration(170, 255, 150) },
            { AnimationType.WALK, new AnimationDuration(480, 510, 425) },
            { AnimationType.GHOST, new AnimationDuration(57, 85, 50) }
        };

        public string GetPathfindingString(List<Node> path)
        {
            Node targetNode = path.Last();

            if (path.Count <= 2)
                return targetNode.GetCharDirection(path.First()) + Hash.Get_Cell_Char(targetNode.Id);

            StringBuilder pathfinder = new StringBuilder();
            char previousDir = path[1].GetCharDirection(path.First()), actualDir;

            for (int i = 2; i < path.Count; i++)
            {
                Node actualNode = path[i];
                Node previousNode = path[i - 1];
                actualDir = actualNode.GetCharDirection(previousNode);

                if (previousDir != actualDir)
                {
                    pathfinder.Append(previousDir);
                    pathfinder.Append(Hash.Get_Cell_Char(previousNode.Id));

                    previousDir = actualDir;
                }
            }
            pathfinder.Append(previousDir);
            pathfinder.Append(Hash.Get_Cell_Char(targetNode.Id));
            return pathfinder.ToString();
        }

        public int GetDeplacementTime(Map map, List<Node> nodes, bool isUsingDrago = false)
        {
            int deplacementTime = 20;
            AnimationDuration animationDuration;
            Cell actualCell = map.Cells[nodes.First().Id];

            if (isUsingDrago)
                animationDuration = AnimationTimes[AnimationType.DRAGO];
            else
                animationDuration = nodes.Count > 6 ? AnimationTimes[AnimationType.RUN] : AnimationTimes[AnimationType.WALK];

            Cell currentCell;

            for (int i = 1; i < nodes.Count; i++)
            {
                currentCell = map.Cells[nodes[i].Id];

                if (actualCell.Y == currentCell.Y)
                    deplacementTime += animationDuration.Horizontal;
                else if (actualCell.X == currentCell.Y)
                    deplacementTime += animationDuration.Vertical;
                else
                    deplacementTime += animationDuration.Linear;

                if (actualCell.LayerGroundLevel < currentCell.LayerGroundLevel)
                    deplacementTime += 100;
                else if (currentCell.LayerGroundLevel > actualCell.LayerGroundLevel)
                    deplacementTime -= 100;
                else if (actualCell.LayerGroundSlope != currentCell.LayerGroundSlope)
                {
                    if (actualCell.LayerGroundSlope == 1)
                        deplacementTime += 100;
                    else if (currentCell.LayerGroundSlope == 1)
                        deplacementTime -= 100;
                }
                actualCell = currentCell;
            }

            return deplacementTime;
        }
    }
}
