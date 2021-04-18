using AivyData.Model.Fights;
using DeepBot.Model.Account.Game.Maps.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Fights
{
    public class MonsterFighter : AbstractFighter
    {
        public short Distance { get; set; }
        public bool IsInvoc { get; set; }
        public MonsterFighter(int id, byte teams, bool isAlive, int health, int healthMax, Cell cell, short pa, short pm, short distance, bool invoc = false) : base(id, teams, isAlive, health, healthMax, cell, pa, pm)
        {
            Distance = distance;
            IsInvoc = invoc;
        }

        public MonsterFighter(int id, byte teams, bool isAlive, int health, int healthMax, Cell cell, short pa, short pm, bool invoc = false) : base(id, teams, isAlive, health, healthMax, cell, pa, pm)
        {
            IsInvoc = invoc;
        }
    }
}
