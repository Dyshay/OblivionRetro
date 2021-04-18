using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Maps
{
    public class CoordinateCell<C> : IMapCell<C>
    {
        private IMapCell<C> Cell;
        private int X;
        private int Y;

        public CoordinateCell(IMapCell<C> cell)
        {
            this.Cell = cell;

            int _loc4 = cell.().dimensions().width();
            int _loc5 = cell.Id() / (_loc4 * 2 - 1);
            int _loc6 = cell.Id() - _loc5 * (_loc4 * 2 - 1);
            int _loc7 = _loc6 % _loc4;

            this.y = _loc5 - _loc7;
            this.x = (cell.Id() - (_loc4 - 1) * this.y) / _loc4;
        }

        public int Id()
        {
            throw new NotImplementedException();
        }

        public bool Walkable()
        {
            throw new NotImplementedException();
        }
    }
}
