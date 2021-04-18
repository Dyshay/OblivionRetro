using AivyData.Model.Fights;
using DeepBot.Model.Account.Game.Maps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DeepBot.Utilities.Pathfinding
{
    public sealed class PathFinder
    {
        private static readonly Lazy<PathFinder> lazy = new Lazy<PathFinder>(() => new PathFinder());

        public static PathFinder Instance { get { return lazy.Value; } }

        private PathFinder() { }

        private Node[] CellPos { get; set; }
        private Map Map { get; set; }

        private void InitGrid()
        {
            var minimap = new int[40, 40];
            short loc3 = 0;
            int loc2;
            int loc5;
            var loc1 = loc2 = loc5 = 0;
            while (loc5 < Map.Height)
            {
                var loc4 = 0;
                while (loc4 < Map.Width)
                {
                    var tmpCell = Map.Cells[loc3];
                    CellPos[loc3] = new Node(loc3, loc1 + loc4, loc2 + loc4, tmpCell.IsWalkable);
                    minimap[loc1 + loc4, 20 + loc2 + loc4] = loc3;
                    loc3++;
                    loc4++;
                }
                loc1++;
                loc4 = 0;
                if (loc3 < Map.Cells.Length)
                {
                    while (loc4 < Map.Width - 1)
                    {
                        var tmpCell = Map.Cells[loc3];
                        CellPos[loc3] = new Node(loc3, loc1 + loc4, loc2 + loc4, tmpCell.IsWalkable);
                        minimap[loc1 + loc4, 20 + loc2 + loc4] = loc3;
                        loc3++;
                        loc4++;
                    }
                }
                loc2--;
                loc5++;
            }
        }

        public List<Node> GetPath(Map map, int startPos, int targetPos, bool useDiag, List<int> obstacles = null)
        {
            var sw = new Stopwatch();
            this.Map = map;
            this.CellPos = new Node[map.Cells.Length];
            InitGrid();

            var startNode = CellPos[startPos];
            var targetNode = CellPos[targetPos];

            var closedSet = new List<Node>();
            var openSet = new List<Node> { startNode };

            while (openSet.Count > 0)
            {
                var index = 0;
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < openSet[index].FCost)
                        index = i;

                    if (openSet[i].FCost != openSet[index].FCost) continue;
                    if (openSet[i].GCost > openSet[index].GCost)
                        index = i;

                    if (useDiag) continue;
                    if (openSet[i].GCost == openSet[index].GCost)
                        index = i;
                }

                var current = openSet[index];

                if (current == targetNode)
                {
                    sw.Stop();
                    return RetracePath(startNode, targetNode);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighbours(current, useDiag))
                {
                    if (closedSet.Contains(neighbor) || !neighbor.Walkable ||(obstacles != null && obstacles.Contains(neighbor.Id))) continue;

                    var tempG = current.GCost + GetDistance(neighbor, current, useDiag);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else if (tempG >= neighbor.GCost)
                        continue;

                    neighbor.GCost = tempG;
                    neighbor.HCost = GetDistance(neighbor, targetNode, useDiag);
                    neighbor.FCost = neighbor.GCost + neighbor.HCost;
                    neighbor.Parent = current;
                }
            }
            return null;
        }

        //private Dictionary<short, Node> GetAccessibleCell(Fight fight, )

        public List<Node> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Add(startNode);
            path.Reverse();
            return path;
        }

        private List<Node> GetNeighbours(Node node, bool useDiagnonal)
        {
            var neighbours = new List<Node>();

            var rightCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X + 1 && nodec.Y == node.Y);
            var leftCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X - 1 && nodec.Y == node.Y);
            var bottomCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X && nodec.Y == node.Y + 1);
            var topCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X && nodec.Y == node.Y - 1);

            if (rightCell != null)
                neighbours.Add(rightCell);
            if (leftCell != null)
                neighbours.Add(leftCell);
            if (bottomCell != null)
                neighbours.Add(bottomCell);
            if (topCell != null)
                neighbours.Add(topCell);

            if (!useDiagnonal) return neighbours;


            var topLeftCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X - 1 && nodec.Y == node.Y - 1);
            var bottomRightCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X + 1 && nodec.Y == node.Y + 1);
            var bottomLeftCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X - 1 && nodec.Y == node.Y + 1);
            var topRightCell = CellPos.FirstOrDefault(nodec => nodec.X == node.X + 1 && nodec.Y == node.Y - 1);

            if (topLeftCell != null)
                neighbours.Add(topLeftCell);
            if (bottomRightCell != null)
                neighbours.Add(bottomRightCell);
            if (bottomLeftCell != null)
                neighbours.Add(bottomLeftCell);
            if (topRightCell != null)
                neighbours.Add(topRightCell);


            return neighbours;
        }

        private int GetDistance(Node a, Node b, bool useDiag)
        {
            if (useDiag)
                return (int)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        }
    }
}
