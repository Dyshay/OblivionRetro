using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Maps
{
    public interface IDofusMap<C> : IMapCell<C>
    {
        public int Size();
        public C Get(int id);

        //public Dimensions Dimensions();
    }
}
