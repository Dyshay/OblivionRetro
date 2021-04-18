using AivyData.Model.Fights;
using AivyDofus.Model.Characters.Characteristics;
using AivyDofus.Model.Characters.Forge;
using AivyDofus.Model.Characters.Inventorys;
using AivyDofus.Model.Characters.Skills;
using AivyDofus.Model.Characters.Spells;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Entities;
using DeepBot.Model.Account.Game.Maps.Interactives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AivyData.Model.Characters
{
    public class Character
    {
        public int Id { get; set; }
        private string _Nickname;
        public short BreedId { get; set; }
        public short Sex { get; set; }
        public string Nickname { get { return _Nickname; } set { _Nickname = value; CharacterNameUpdate?.Invoke(); } }
        public int Level { get; set; }
        public MagicForge Forge { get; set; }

        public int Kamas { get; set; }
        public int BoostPoints { get; set; }
        public Dictionary<int, Spell> Spells { get; set; }
        public Characteristic Characteristics { get; set; }
        public Inventory Inventory { get; set; }
        public short ActualPods { get; set; }
        public short MaxPods { get; set; }
        public bool IsAutoClick { get; set; }
        public Map Map { get; set; }
        public Cell Cell { get; set; }
        public List<Skill> Skills { get; set; }
        public Fight Fight { get; set; }
        private Account Account { get; set; }
        public event Action CharacterNameUpdate;
        public Character(Account account)
        {
            Map = new Map();
            Cell = new Cell();
            Spells = new Dictionary<int, Spell>();
            Characteristics = new Characteristic();
            Fight = new Fight(account);
            Account = account;
            Skills = new List<Skill>();
            Inventory = new Inventory(account);
            Forge = new MagicForge();
        }

        public bool IsOnMap(string mapId) => Map.MapId == int.Parse(mapId);
        public void ActualizeSpellList(string package)
        {
            Spells.Clear();
            var datas = package.Replace("_;", "_").Split(';');
            foreach (var dat in datas)
            {
                var split = dat.Split('~');
                if (split.Length >= 1)
                {
                    //short spellId = short.Parse(split[0]);
                    //var spell = Spell.Spells[spellId].Clone();
                    //spell.Level = byte.Parse(split[1]);
                    //Spells.Add(spellId, spell);
                }
            }
        }

        public void ActualizeCharacteristics(string package)
        {
            string[] _loc3 = package.Substring(2).Split('|');
            Kamas = int.Parse(_loc3[1]);
            BoostPoints = int.Parse(_loc3[2]);

            Characteristics.UpdateCharacteristics(package);
            Account.AccountStateUpdate.Invoke();
        }

        public List<Monster> GetMonsters() => Map.Entities.Values.Where(monster => monster is Monster).Select(monster => monster as Monster).ToList();
        public List<Monster> GetGroupsMonster(int monster_level_min, int monster_level_max)
        {
            var groups = GetMonsters();
            var test = groups.Where(c => c.GroupLevel > monster_level_min && c.GroupLevel < monster_level_max).ToList();
            return groups.Where(c => c.GroupLevel > monster_level_min && c.GroupLevel < monster_level_max).ToList();
        }

        public void OnChangeMap(Map newMap)
        {
            Map = newMap;
            for (int i = 0; i < Map.Interactives.Values.ToArray().Length; i++)
            {
                Map.Interactives.Values.ToArray()[i].IsUsable = true;
                Map.Interactives.Values.ToArray()[i].IdSkill = InteractivObject.InteractivesObjects[Map.Interactives.Values.ToArray()[i].Id].IdSkill;
            }
            Map.Pathmaker.SetMap(Map);
        }
    }
}
