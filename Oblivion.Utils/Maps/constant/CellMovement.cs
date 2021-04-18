using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Maps.constant
{
    public enum CellState
    {
        NOT_WALKABLE,
        NOT_WALKABLE_INTERACTIVE,
        TRIGGER,
        LESS_WALKABLE,
        DEFAUT,
        PADDOCK,
        ROAD,
        MOST_WALKABLE 
    }

    public class CellMovement
    {
        private static CellState[] values = Enum.GetValues(typeof(CellState)).Cast<CellState>().ToArray();
        private CellState CurrentState;

        public CellMovement(CellState state)
        {
            CurrentState = state;
        }
        public bool walkable()
        {
            return (int)CurrentState > (int)CellState.NOT_WALKABLE_INTERACTIVE;
        }

        public static CellState byValue(int value)
        {
            return values[value];
        }
    }
}
