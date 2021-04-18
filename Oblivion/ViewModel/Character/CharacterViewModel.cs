using AivyData.Model;
using AivyDofus.Model.Characters.Characteristics;
using Oblivion.Utility.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Oblivion.ViewModel.Character
{
    public class CharacterViewModel : StackNavigator
    {
        private Characteristic _Characteristics;
        public Characteristic Characteristics
        {
            get { return _Characteristics; }
            set { _Characteristics = value; RaisePropertyChanged(); }
        }

        private BitmapImage _ImageSource;
        public BitmapImage ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; RaisePropertyChanged(); }
        }


        private Account _account;

        public CharacterViewModel(Characteristic stats, Account account)
        {
            Characteristics = stats;
            _account = account;
            _account.AccountStateUpdate += HandleRefresh;
        }

        private void HandleRefresh()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                RaisePropertyChanged(string.Empty);
                if (_account.Character.BreedId != 0)
                {
                    BitmapImage image = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}Resources/Pictures/{_account.Character.BreedId.ToString() + _account.Character.Sex.ToString()}.png"));
                    image.Freeze();
                    ImageSource = image;
                }
            }));
        }
    }
}
