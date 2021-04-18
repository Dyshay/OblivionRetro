using AivyData.Model.Fights;
using AivyDofus.Model.Characters.Characteristics;
using AivyDofus.Model.Characters.Spells;
using DeepBot.Model.Account.Game.Maps.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Fights
{
    public class CharacterFighter : AbstractFighter
    {
        public Characteristic Stats { get; set; }
        public Dictionary<int, Spell> Spells { get; set; }
        public CharacterFighter(int id, byte teams, bool isAlive, int health, int healthMax, Cell cell, short pa, short pm, Characteristic stats, Dictionary<int, Spell> spells) : base(id, teams, isAlive, health, healthMax, cell, pa, pm)
        {
            Stats = stats;
            Spells = spells;
        }
    }
}
