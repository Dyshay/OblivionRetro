using System.Collections.Generic;
using System.Linq;

namespace DeepBot.Model.Account.Game.Maps.Entities
{
    public class Monster : Entity
    {
        public int Template { get; set; }
        public int Level { get; set; }

        public List<Monster> Group { get; set; } = new List<Monster>();
        public int MonstersNumber => Group.Count + 1;
        public int GroupLevel => Level + Group.Sum(f => f.Level);
    }
}
