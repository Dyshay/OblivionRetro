using AivyData.Enums.Map;
using DeepBot.Model.Account.Game.Maps.Entities;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepBot.Model.Account.Game.Maps.Cells
{
    public class Cell
    {
        public short CellId { get; set; }
        public bool IsActive { get; set; }
        public CellTypes Type { get; set; }
        public bool IsInLineOfSight { get; set; }
        public byte? LayerGroundLevel { get; set; }
        public byte LayerGroundSlope { get; set; }
        public short LayerObject1 { get; set; }
        public short LayerObject2 { get; set; }
        public short InteractiveObject { get; set; }
        public int X { get; set; }
        public bool isRight { get; set; }
        public bool isLeft { get; set; }
        public bool isBottom { get; set; }
        public bool isTop { get; set; }
        public int Y { get; set; }

        public int Z { get; set; }

        public static readonly int[] TeleportTexturesSpritesId = { 1030, 1029, 1764, 2298, 745 };

        public bool IsTeleportCell => TeleportTexturesSpritesId.Contains(LayerObject1) || TeleportTexturesSpritesId.Contains(LayerObject2);
        public bool IsInteractiveCell => Type == CellTypes.INTERACTIVE_OBJECT || InteractiveObject != -1;
        public bool IsWalkable => IsActive && Type != CellTypes.NOT_WALKABLE && !IsInLineOfSight;

        public int GetDistanceBetweenCells(Cell prmDestinationCell) => Math.Abs(X - prmDestinationCell.X) + Math.Abs(Y - prmDestinationCell.Y);


    }
    public static class CellExtensions
    {
        public static List<Cell> GetAdjacents(this Cell cell, Cell[] map_cells, int movePointAvailable)
        {
            List<Cell> Cells_Right = new List<Cell>();
            List<Cell> Cells_Left = new List<Cell>();
            List<Cell> Cells_Bottom = new List<Cell>();
            List<Cell> Cells_Top = new List<Cell>();
            for (int i = 1; i < movePointAvailable + 1; i++)
            {
                Cell cell_right = map_cells.FirstOrDefault(cellules => cellules.X == cell.X + i && cellules.Y == cell.Y);
                Cell cell_left = map_cells.FirstOrDefault(cellules => cellules.X == cell.X - i && cellules.Y == cell.Y);
                Cell cell_down = map_cells.FirstOrDefault(cellules => cellules.X == cell.X && cellules.Y == cell.Y + i);
                Cell cell_top = map_cells.FirstOrDefault(cellules => cellules.X == cell.X && cellules.Y == cell.Y - i);

                if (i > 1)
                {
                    if (cell_right != null && Cells_Right.Last().IsWalkable)
                        Cells_Right.Add(cell_right);
                    if (cell_left != null && Cells_Left.Last().IsWalkable)
                        Cells_Left.Add(cell_left);
                    if (cell_down != null && Cells_Bottom.Last().IsWalkable)
                        Cells_Bottom.Add(cell_down);
                    if (cell_top != null && Cells_Top.Last().IsWalkable)
                        Cells_Top.Add(cell_top);
                }
                else
                {
                    if (cell_right != null)
                        Cells_Right.Add(cell_right);
                    if (cell_left != null)
                        Cells_Left.Add(cell_left);
                    if (cell_down != null)
                        Cells_Bottom.Add(cell_down);
                    if (cell_top != null)
                        Cells_Top.Add(cell_top);
                }
            }

            return Cells_Right.Concat(Cells_Top).Concat(Cells_Left).Concat(Cells_Bottom).ToList();
        }

        public static List<Cell> GetAdjecentsWithSpecific(this Cell cell, Cell[] map_cells, int movePoints)
        {
            List<Cell> Cell_Adjacents = new List<Cell>();

            Cell cell_right = map_cells.FirstOrDefault(cellules => cellules.X == cell.X + movePoints && cellules.Y == cell.Y);
            Cell cell_left = map_cells.FirstOrDefault(cellules => cellules.X == cell.X - movePoints && cellules.Y == cell.Y);
            Cell cell_down = map_cells.FirstOrDefault(cellules => cellules.X == cell.X && cellules.Y == cell.Y + movePoints);
            Cell cell_top = map_cells.FirstOrDefault(cellules => cellules.X == cell.X && cellules.Y == cell.Y - movePoints);

            if (cell_right != null && cell_right.IsWalkable)
                Cell_Adjacents.Add(cell_right);
            if (cell_left != null && cell_left.IsWalkable)
                Cell_Adjacents.Add(cell_left);
            if (cell_down != null && cell_down.IsWalkable)
                Cell_Adjacents.Add(cell_down);
            if (cell_top != null && cell_top.IsWalkable)
                Cell_Adjacents.Add(cell_top);

            return Cell_Adjacents;
        }

        public static bool IsInLine(this Cell cell, Cell destinationCell, Cell[] map_cells, int distance)
        {
            var rightCells = cell.GetCellsInLine(map_cells, 1, distance);
            var leftCells = cell.GetCellsInLine(map_cells, 2, distance);
            var bottomCells = cell.GetCellsInLine(map_cells, 3, distance);
            var topCells = cell.GetCellsInLine(map_cells, 4, distance);
            if (rightCells.Select(c => c.CellId).Contains(destinationCell.CellId))
            {
                return rightCells.IsLineOfSight(destinationCell);
            }
            else if (leftCells.Select(c => c.CellId).Contains(destinationCell.CellId))
            {
                return leftCells.IsLineOfSight(destinationCell);
            }
            else if (bottomCells.Select(c => c.CellId).Contains(destinationCell.CellId))
            {
                return bottomCells.IsLineOfSight(destinationCell);
            }
            else if (topCells.Select(c => c.CellId).Contains(destinationCell.CellId))
            {
                return topCells.IsLineOfSight(destinationCell);
            }
            else if (!topCells.Select(c => c.CellId).Contains(destinationCell.CellId)
                && !bottomCells.Select(c => c.CellId).Contains(destinationCell.CellId)
                && !leftCells.Select(c => c.CellId).Contains(destinationCell.CellId) && !rightCells.Select(c => c.CellId).Contains(destinationCell.CellId))
                return false;

            return false;
        }

        private static List<Cell> GetCellsInLine(this Cell cell, Cell[] map_cells, int direction, int distance)
        {
            List<Cell> Cells = new List<Cell>();
            for (int i = 1; i < distance + 1; i++)
            {
                switch (direction)
                {
                    case 1:
                        Cells.Add(map_cells.FirstOrDefault(cellules => cellules.X == cell.X + i && cellules.Y == cell.Y));
                        break;
                    case 2:
                        Cells.Add(map_cells.FirstOrDefault(cellules => cellules.X == cell.X - i && cellules.Y == cell.Y));
                        break;
                    case 3:
                        Cells.Add(map_cells.FirstOrDefault(cellules => cellules.X == cell.X && cellules.Y == cell.Y + i));
                        break;
                    case 4:
                        Cells.Add(map_cells.FirstOrDefault(cellules => cellules.X == cell.X && cellules.Y == cell.Y - i));
                        break;
                    default:
                        break;
                }
            }
            return Cells.Where(c => c != null).ToList();
        }

        private static bool IsLineOfSight(this IEnumerable<Cell> cells, Cell cell_destination)
        {
            foreach (var item in cells)
            {
                if ((item.Type == CellTypes.NOT_WALKABLE || item.Type == CellTypes.INTERACTIVE_OBJECT) && !item.IsActive)
                    return false;
                if (item.CellId == cell_destination.CellId)
                    return true;
            }
            return false;
        }


        public static Node ToNode(this Cell cell)
        {
            return new Node(cell.CellId, cell.X, cell.Y, cell.IsWalkable);
        }

    }
}
