using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Maps.constant
{
    public enum DirectionEnum
    {
        EAST,
        SOUTH_EAST,
        SOUTH,
        SOUTH_WEST,
        WEST,
        NORTH_WEST,
        NORTH,
        NORTH_EAST
    }
    public class Direction
    {
        private static DirectionEnum[] values = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>().ToArray();
        public DirectionEnum CurrentDirection;

        private Func<int, int> computeNextCell;

        public Direction(Func<int, int> computeNextCell, DirectionEnum direction)
        {
            CurrentDirection = direction;
            this.computeNextCell = computeNextCell;
        }

        public char toChar()
        {
            return (char)((CurrentDirection) + 'a');
        }

        public DirectionEnum opposite()
        {
            return values[(int)(CurrentDirection + 4) % 8];
        }

        public DirectionEnum orthogonal()
        {
            return values[(int)(CurrentDirection + 2) % 8];
        }

        public bool restricted()
        {
            return (int)CurrentDirection % 2 == 1;
        }

        public int nextCellIncrement(int mapWidth)
        {
            return computeNextCell.Invoke(mapWidth);
        }

       public static DirectionEnum byChar(char c)
        {
            return values[c - 'a'];
        }
    }

    public static class DirectionExtension
    {
        public static int EAST(this int width)
        {
            return 1;
        }

        public static int SOUTH_EAST(this int width)
        {
            return width;
        }

        public static int SOUTH(this int width)
        {
            return 2 * width - 1;
        }
        public static int SOUTH_WEST(this int width)
        {
            return width - 1;
        }
        public static int WEST(this int width)
        {
            return 1;
        }
        public static int NORTH_WEST(this int width)
        {
            return -width;
        }
        public static int NORTH(this int width)
        {
            return -(2 * width - 1);
        }
        public static int NORTH_EAST(this int width)
        {
            return -(width - 1);
        }
    }
}
