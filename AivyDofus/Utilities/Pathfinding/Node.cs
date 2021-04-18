using System;
using System.Collections.Generic;
using System.Text;

namespace DeepBot.Utilities.Pathfinding
{
    public class Node
    {
        public Node(short id, int x, int y, bool w)
        {
            Id = id;
            X = x;
            Y = y;
            Walkable = w;
        }

        public short Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool Walkable { get; set; }

        public Node Parent { get; set; }

        public int HCost { get; set; }
        public int GCost { get; set; }
        public int FCost { get; set; }

        public char GetCharDirection(Node prmNode)
        {
            if (X == prmNode.X)
                return prmNode.Y < Y ? (char)(7 + 'a') : (char)(3 + 'a');
            else if (Y == prmNode.Y)
                return prmNode.X < X ? (char)(1 + 'a') : (char)(5 + 'a');
            else if (X > prmNode.X)
                return Y > prmNode.Y ? (char)(0 + 'a') : (char)(2 + 'a');
            else if (X < prmNode.X)
                return Y < prmNode.Y ? (char)(4 + 'a') : (char)(6 + 'a');
            throw new Exception("Error direction non trouvée");
        }
    }
}
