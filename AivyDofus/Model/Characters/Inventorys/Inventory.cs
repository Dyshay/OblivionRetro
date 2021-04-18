using AivyData.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Inventorys
{
    public class Inventory
    {
        private Account Account;
        public ConcurrentDictionary<int, Item> Items { get; set; }
        public IEnumerable<Item> ObjectItems => Items.Values;

        public event Action InventoryRefresh;

        public Inventory(Account acc)
        {
            Items = new ConcurrentDictionary<int, Item>();
            Account = acc;
        }

        public void getInventory(string package)
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (string data in package.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            string[] values = data.Split('~');
                            Item @object = new Item(data);
                            Items.TryAdd(@object.GuidItem, @object);
                        }
                    }

                }
                catch (Exception)
                {

                }
            }).Wait();

            InventoryRefresh?.Invoke();
        }

        public void RemoveItem(string package, int quantity, bool isDeletePackage)
        {
            int.TryParse(package.Substring(2), out int GuidItem);

            if (Items.TryGetValue(GuidItem, out Item deleteItem))
            {
                if (deleteItem.Quantity > quantity)
                {
                    deleteItem.Quantity -= quantity;
                    Items[GuidItem] = deleteItem;
                }
                else
                {
                    Items.TryRemove(GuidItem, out deleteItem);
                }

                if (isDeletePackage)
                {
                    Account.Send($"Od{deleteItem.GuidItem}|{deleteItem.Quantity}");
                }
                InventoryRefresh?.Invoke();
            }
        }

        public void UpdateItem(string package)
        {
            string newPackage = package.Substring(2);

            if (!string.IsNullOrEmpty(newPackage))
            {
                string[] separator = newPackage.Split('|');

                if (Items.TryGetValue(int.Parse(separator[0]), out Item item))
                {
                    int quantity = int.Parse(separator[1]);

                    item.Quantity = quantity;

                    Items[item.GuidItem].Quantity = quantity;

                    InventoryRefresh?.Invoke();
                }
            }
        }
    }
}
