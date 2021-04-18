using AivyData.Model;
using AivyDofus.Model.Characters.Inventorys;
using Oblivion.Utility.Navigator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBoxNET.MVVM.Command;

namespace Oblivion.ViewModel.Character
{
    public class MagicForgeViewModel : StackNavigator
    {
        private ObservableCollection<Item> _Items;
        public ObservableCollection<Item> Items
        {
            get { return _Items; }
            set { _Items = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Item> _Runes;
        public ObservableCollection<Item> Runes
        {
            get { return _Runes; }
            set { _Runes = value; RaisePropertyChanged(); }
        }

        private Item _ModifyItem;
        public Item ModifyItem
        {
            get { return _ModifyItem; }
            set { _ModifyItem = value; RaisePropertyChanged(); ModifyItemIsSelected = value != null ? true : false; }
        }

        private bool _ModifyItemIsSelected;

        public bool ModifyItemIsSelected
        {
            get { return _ModifyItemIsSelected; }
            set { _ModifyItemIsSelected = value; RaisePropertyChanged(); }
        }

        private ICommand _UpdateButton;

        public ICommand UpdateButton
        {
            get { return _UpdateButton ?? (_UpdateButton = new RelayCommandAsync(ExecuteUpdate, CanExecuteUpdate)); }
        }

        private bool CanExecuteUpdate(object obj)
        {
            return ModifyItem != null && SwitchNumber > 0;
        }

        private ICommand _StopButton;

        public ICommand StopButton
        {
            get { return _StopButton ?? (_StopButton = new RelayCommand(ExecuteStop, CanExecuteStop)); }
        }

        //private Predicate<bool> CanExecuteUpdate()
        //{
        //    return ModifyItemIsSelected && SwitchNumber > 0;
        //}

        private void ExecuteStop()
        {
            Account.Character.Forge.Stop();
        }

        private bool CanExecuteStop()
        {
            return Account.Character.Forge.IsStart;
        }

        private async Task ExecuteUpdate()
        {
            Account.Character.Forge = new AivyDofus.Model.Characters.Forge.MagicForge(Runes.ToList(), ModifyItem, SwitchNumber, Account);

            await Account.Character.Forge.GoTable();
            await Account.Character.Forge.StartFM();
        }

        private int _SwitchNumber;

        public int SwitchNumber
        {
            get { return _SwitchNumber; }
            set { _SwitchNumber = value; RaisePropertyChanged(); }
        }



        private Account Account;

        public MagicForgeViewModel(Account _account)
        {
            Account = _account;
            Account.Character.Inventory.InventoryRefresh += HandleInventory;
            Items = new ObservableCollection<Item>();
            Runes = new ObservableCollection<Item>();
        }

        private void HandleInventory()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Items = new ObservableCollection<Item>(Account.Character.Inventory.ObjectItems.Where(c => c.Type == ItemTypeEnum.TYPE_AMULETTE));
                Runes = new ObservableCollection<Item>(Account.Character.Inventory.ObjectItems.Where(c => c.Type == ItemTypeEnum.TYPE_RUNE_FORGEMAGIE));
            });
        }
    }
}
