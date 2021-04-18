using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Spells
{
    public class Spell
    {
        public static Dictionary<short, Spell> Spells { get; set; }
        public short Id { get; set; }
        public string Name { get; set; }
        public short Level { get; set; }
        public string LevelOne { get; set; }
        public string LevelTwo { get; set; }
        public string LevelThree { get; set; }
        public string LevelFour { get; set; }
        public string LevelFive { get; set; }
        public string LevelSix { get; set; }
        public Dictionary<byte, SpellStat> Stats;
        public static void Initialize()
        {
            Spells = new Dictionary<short, Spell>();
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Spells.json"))
            {
                string result = reader.ReadToEnd();
                Spells = JsonConvert.DeserializeObject<List<Spell>>(result).ToDictionary(c => c.Id, c => DecompressSpell(c)); ;
            }
        }

        public static Spell DecompressSpell(Spell spell)
        {
            spell.Stats = new Dictionary<byte, SpellStat>();
            spell.Stats.Add(1, DecompressSpellStat(spell.LevelOne));
            spell.Stats.Add(2, DecompressSpellStat(spell.LevelTwo));
            spell.Stats.Add(3, DecompressSpellStat(spell.LevelThree));
            spell.Stats.Add(4, DecompressSpellStat(spell.LevelFour));
            spell.Stats.Add(5, DecompressSpellStat(spell.LevelFive));
            spell.Stats.Add(6, DecompressSpellStat(spell.LevelSix));
            return spell;
        }

        public static SpellStat DecompressSpellStat(string data)
        {
            if (data == "-1")
                return null;
            var stat = new SpellStat();
            var datas = data.Split(',');
            //stat.NormalEffects = DecompressSpellEffect(datas[0]);
            //stat.CriticalEffects = DecompressSpellEffect(datas[1]);
            stat.PA = byte.Parse(datas[2]);
            stat.RangeMinimum = byte.Parse(datas[3]);
            stat.RangeMaximum = byte.Parse(datas[4]);
            stat.CriticalRate = short.Parse(datas[5]);
            stat.EchecRate = short.Parse(datas[6]);
            stat.LineOnly = bool.Parse(datas[7]);
            stat.LineOfSight = bool.Parse(datas[8]);
            stat.FreeCell = bool.Parse(datas[9]);
            stat.CanBoostRange = bool.Parse(datas[10]);
            stat.LaunchCountByTurn = byte.Parse(datas[11]);
            stat.LaunchCountByTarget = byte.Parse(datas[12]);
            stat.CoolDown = byte.Parse(datas[14]);
            stat.RangeType = datas[15];
            stat.RequiredLevel = byte.Parse(datas[18]);
            return stat;
        }

        public Spell Clone()
        {
            return new Spell()
            {
                Id = Id,
                Name = Name,
                Stats = Stats,
            };
        }
    }
}
