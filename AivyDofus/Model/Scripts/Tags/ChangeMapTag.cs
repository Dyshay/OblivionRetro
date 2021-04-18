using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Scripts.Tags
{
    public class ChangeMapTag : ITag
    {
        public int CellID { get; set; }
        public ChangeMapTag(int cellId) => CellID = cellId;
    }
}
