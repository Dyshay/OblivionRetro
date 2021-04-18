using AivyDofus.Model.Characters.Effects;
using X = System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Inventorys
{
    public static class ItemExtensions
    {
        public static List<Effect> GetEffects(this string effectData)
        {
            List<Effect> Effects = new List<Effect>();
            string[] effect_split = effectData.Split(',');
            for (int i = 0; i < effect_split.Length; i++)
            {
                string[] stats = effect_split[i].Split('#');
                if (!string.IsNullOrEmpty(stats[0]))
                {
                    EffectEnum type = (EffectEnum)X.Convert.ToInt32(stats[0], 16);
                    if (stats[stats.Length - 1].Length > 4)
                    {
                        Effects.Add(new Effect(type, stats[stats.Length - 1].Length > 1 ? stats[stats.Length - 1].Substring(3) : stats[stats.Length - 1]));
                    }
                }
            }
            return Effects;
        }

        public static List<Item> ToItem(this List<ItemJson> items)
        {
            List<Item> itemsModel = new List<Item>();
            items.ForEach(c =>
            {
                Item item = new Item()
                {
                    Id = (uint)c.Id,
                    Name = c.Name
                };
                itemsModel.Add(item);
            });
            return itemsModel;
        }
    }
}
