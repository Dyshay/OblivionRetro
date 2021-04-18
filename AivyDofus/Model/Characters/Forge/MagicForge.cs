using AivyData.Model;
using AivyDofus.Model.Characters.Inventorys;
using DeepBot.Utilities.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Forge
{
    public class MagicForge
    {
        private List<Item> RunesAvailables;
        public Item CurrentItem;

        private int RequestNumber;
        private int CurrentNumber;
        public bool IsStart;
        public bool IsChangeItems;
        private bool IsItemPlace;
        private Account _account;
        private Item CurrentRune;

        public MagicForge()
        {

        }

        public MagicForge(List<Item> runes, Item item, int switchNumber, Account account)
        {
            RunesAvailables = runes;
            CurrentItem = item;
            RequestNumber = switchNumber;
            _account = account;
        }

        public void Stop()
        {
            IsStart = false;
        }


        public async Task GoTable()
        {
            var nodes = PathFinder.Instance.GetPath(_account.Character.Map, _account.Character.Cell.CellId, 357, true);
            var pathString = PathFinderUtils.Instance.GetPathfindingString(nodes);
            _account.Send($"GA001{pathString}");
            await Task.Delay(PathFinderUtils.Instance.GetDeplacementTime(_account.Character.Map, nodes));
            _account.Send("GA500" + 357 + ";" + 169);
            IsStart = true;
        }

        public async Task StartFM()
        {

            await Task.Delay(50);

            if (RunesAvailables.Any(rune => rune.Quantity == 1))
            {
                IsStart = false;
                _account.Send("EV");
                return;
            }

            if (IsStart)
            {
                if (!IsChangeItems)
                {
                    await Task.Delay(50);

                    if (RequestNumber == CurrentNumber || CurrentNumber == 0)
                        await SetRune();

                    if (!IsItemPlace)
                        await SetItem();

                    await Task.Delay(50);

                    FM();
                }
            }

            if (!IsStart)
            {
                _account.Send("EV");
                return;
            }

            await StartFM();
        }

        private async Task SetItem()
        {
            _account.Send($"EMO+{CurrentItem.GuidItem}|{CurrentItem.Quantity}");
            _account.SendFromServer($"EMKO+{CurrentItem.GuidItem}|{CurrentItem.Quantity}");
            await Task.Delay(50);
        }

        private void FM()
        {
            _account.Send("EK");
            CurrentNumber += 1;
        }

        private async Task SetRune()
        {
            if (CurrentItem != null && CurrentNumber == RequestNumber)
            {
                _account.Send($"EMO-{CurrentRune.GuidItem}|{CurrentRune.Quantity}");
                _account.SendFromServer($"EMKO-{CurrentRune.GuidItem}|{CurrentRune.Quantity}");
                CurrentNumber = 0;
                await Task.Delay(50);
            }
            if (CurrentRune != null)
            {
                CurrentRune = RunesAvailables.First(c => c.GuidItem != CurrentRune.GuidItem);
                _account.Send($"EMO+{CurrentRune.GuidItem}|{CurrentRune.Quantity}");
                _account.SendFromServer($"EMKO+{CurrentRune.GuidItem}|{CurrentRune.Quantity}");
                await Task.Delay(50);
            }
            if (CurrentRune == null)
            {
                CurrentRune = RunesAvailables.First();
                _account.Send($"EMO+{CurrentRune.GuidItem}|{CurrentRune.Quantity}");
                _account.SendFromServer($"EMKO+{CurrentRune.GuidItem}|{CurrentRune.Quantity}");
                await Task.Delay(50);
            }

        }
    }
}
