using AivyDofus.Model.Characters.Effects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Inventorys
{
    public class Item
    {
        public uint Id { get; set; }
        public int GuidItem { get; set; }
        public string Name { get; set; }
        public short Level { get; set; }
        public ItemTypeEnum Type { get; set; }
        public ItemSlotEnum Position { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public bool IsUsable { get; set; }
        public bool IsEquippable => IsEquippabled();
        public List<Effect> Effects { get; set; }

        public Item(string data)
        {
            Effects = new List<Effect>();
            getInventoryItem(data);
        }
        internal Item()
        {

        }

        public void getInventoryItem(string package)
        {
            string[] values = package.Split('~');

            if (values.Length <= 2)
                return;
            GuidItem = Convert.ToInt32(values[0], 16);
            Id = Convert.ToUInt32(values[1], 16);
            Quantity = Convert.ToInt32(values[2], 16);
            if (int.TryParse(values[3], out int data) || string.IsNullOrEmpty(values[3]))
                Position = string.IsNullOrEmpty(values[3]) ? ItemSlotEnum.SLOT_INVENTORY : (ItemSlotEnum)Convert.ToInt32(values[3]);

            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Items.json"))
            {
                string result = reader.ReadToEnd();
                var Item = JsonConvert.DeserializeObject<List<ItemJson>>(result).FirstOrDefault(c => c.Id == Id);
                if (Item != null)
                {
                    Effects = values[4].GetEffects();
                    Name = Item.Name;
                    Level = Item.Level;
                    Weight = (Quantity * Item.Weight);
                    IsUsable = Item.Usable;
                    Type = (ItemTypeEnum)Item.Type;
                }
            }
        }

        public bool IsEquippabled()
        {
            return ItemSlotEnum.SLOT_INVENTORY == Position && (ItemTypeEnum.TYPE_PIERRE_AME == Type || ItemTypeEnum.TYPE_PELLE == Type || ItemTypeEnum.TYPE_OUTIL == Type || ItemTypeEnum.TYPE_PIOCHE == Type || ItemTypeEnum.TYPE_COIFFE == Type
                || ItemTypeEnum.TYPE_ANNEAU == Type || ItemTypeEnum.TYPE_AMULETTE == Type || ItemTypeEnum.TYPE_BOTTES == Type || ItemTypeEnum.TYPE_CEINTURE == Type || ItemTypeEnum.TYPE_DAGUES == Type || ItemTypeEnum.TYPE_DOFUS == Type
                || ItemTypeEnum.TYPE_EPEE == Type || ItemTypeEnum.TYPE_FAUX == Type || ItemTypeEnum.TYPE_HACHE == Type || ItemTypeEnum.TYPE_BATON == Type || ItemTypeEnum.TYPE_BAGUETTE == Type || ItemTypeEnum.TYPE_ARC == Type
                );
        }
    }
}
