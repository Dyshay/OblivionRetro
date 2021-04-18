using DeepBot.Model.Account.Game.Maps.Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepBot.Model.Account.Game.Maps.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public Cell Cell { get; set; }
        public virtual string ToolTip { get; set; }
    }
}
