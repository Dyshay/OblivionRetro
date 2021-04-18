using DeepBot.Model.Account.Game.Maps.Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace AivyData.Model.Fights
{
    public abstract class AbstractFighter
    {
        public int Id { get; set; }
        public byte Teams { get; set; }
        public bool IsAlive { get; set; }
        public int Health { get; set; }
        public int HealthMax { get; set; }
        public Cell Cell { get; set; }
        public short Pa { get; set; }
        public short Pm { get; set; }
        public int IdInvocator { get; set; }
        public int HealthPercentage => (int)((double)Health / HealthMax) / 100;

        public AbstractFighter(int id, byte teams, bool isAlive, int health, int healthMax,Cell cell, short pa, short pm)
        {
            Id = id;
            Teams = teams;
            IsAlive = isAlive;
            Health = health;
            HealthMax = healthMax;
            Cell = cell;
            Pa = pa;
            Pm = pm;
        }
    }
}
