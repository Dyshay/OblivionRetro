using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Maps
{
    public interface IMapCell<C>
    {
        public int Id();
        public bool Walkable();
    }
}
