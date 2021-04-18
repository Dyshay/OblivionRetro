using AivyData.Model;
using AivyDofus.Model.Characters.Inventorys;
using Oblivion.Utility.Navigator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.ViewModel.Character
{
    public class InventoryViewModel : StackNavigator
    {
        private ObservableCollection<Item> _Objects;
        public ObservableCollection<Item> Objects
        {
            get { return _Objects; }
            set { _Objects = value; RaisePropertyChanged(); }
        }
        private Account Account;

        public InventoryViewModel(Account _account)
        {
            Account = _account;
            Objects = new ObservableCollection<Item>(Account.Character.Inventory.ObjectItems);
            Account.Character.Inventory.InventoryRefresh += HandleRefreshInventory;
        }

        private void HandleRefreshInventory()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Objects = new ObservableCollection<Item>(Account.Character.Inventory.ObjectItems);
            });
        }
    }
}
