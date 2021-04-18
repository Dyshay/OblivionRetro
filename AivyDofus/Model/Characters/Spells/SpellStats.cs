using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Spells
{
    public class SpellStat
    {
        public byte PA { get; set; }
        public byte RangeMinimum { get; set; }
        public byte RangeMaximum { get; set; }
        public short CriticalRate { get; set; }
        public short EchecRate { get; set; }

        public bool LineOnly { get; set; }
        public bool LineOfSight { get; set; }
        public bool FreeCell { get; set; }
        public bool CanBoostRange { get; set; }

        public byte LaunchCountByTurn { get; set; }
        public byte LaunchCountByTarget { get; set; }
        public byte CoolDown { get; set; }
        public byte RequiredLevel { get; set; }

        public string RangeType { get; set; }
    }
}
