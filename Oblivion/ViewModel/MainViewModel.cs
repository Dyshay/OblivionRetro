using AivyData.Enums;
using AivyData.Model;
using AivyDofus.Model.Characters.Spells;
using AivyDofus.Proxy;
using AivyDofus.Server;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Interactives;
using MahApps.Metro.Controls;
using NLog;
using Oblivion.Model.Accounts;
using Oblivion.Utility.Navigator;
using Oblivion.View.Logs;
using Oblivion.View.Settings;
using Oblivion.ViewModel.Character;
using Oblivion.ViewModel.Logs;
using Oblivion.ViewModel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ToolBoxNET.MVVM.Base;
using ToolBoxNET.MVVM.Command;

namespace Oblivion.ViewModel
{
    public class MainViewModel : StackNavigator
    {
        private string _CharacterName;

        public string CharacterName
        {
            get { return _CharacterName; }
            set { _CharacterName = value; RaisePropertyChanged(); }
        }

        private DofusRetroProxy _Proxy;

        public DofusRetroProxy Proxy
        {
            get { return _Proxy; }
            set { _Proxy = value; RaisePropertyChanged(); }
        }

        public AccountState State
        {
            get { return Proxy.Account.State; }
        }

        public bool AccountIsConnected
        {
            get { return !Proxy.Account.IsConnected; }
        }

        public string Kamas
        {
            get { return Proxy.Account.Character.Kamas.ToString() + " Kamas"; }
        }

        public string Level
        {
            get { return "Level " + Proxy.Account.Character.Level.ToString(); }
        }

        public string Pods
        {
            get { return $"{Proxy.Account.Character.ActualPods}/{Proxy.Account.Character.MaxPods} Pods"; }
        }

        public string Vitality
        {
            get { return $"{Proxy.Account.Character.Characteristics.Health}/{Proxy.Account.Character.Characteristics.HealthMax} vie"; }
        }

        private SettingViewModel _settingViewModel;
        public  SettingViewModel SettingViewModel
        {
            get { return _settingViewModel; }
            set { _settingViewModel = value; RaisePropertyChanged(); }
        }

        private LogViewModel _logViewModel;
        public LogViewModel LogViewModel
        {
            get { return _logViewModel; }
            set { _logViewModel = value; RaisePropertyChanged(); }
        }


        private CharacterViewModel _CharacterViewModel;
        private InventoryViewModel _InventoryViewModel;
        private MagicForgeViewModel _MagicForgeViewModel;

        private ListBoxItem _SelectedView;
        public ListBoxItem SelectedView
        {
            get { return _SelectedView; }
            set { _SelectedView = value; RaisePropertyChanged(); UpdateViewMenu(); }
        }

        private ICommand _RequestConnection;
        public ICommand RequestConnection
        {
            get { return _RequestConnection ?? (_RequestConnection = new RelayCommand(ProcessConnection, CanProcessConnection)); }
        }

        private ICommand _RequestSettingWindow;
        public ICommand RequestSettingWindow
        {
            get { return _RequestSettingWindow ?? (_RequestSettingWindow = new RelayCommand(OpenSettingWindow)); }
        }

        private ICommand _RequestLogWindow;
        public ICommand RequestLogWindow
        {
            get { return _RequestLogWindow ?? (_RequestLogWindow = new RelayCommand(OpenLogWindow)); }
        }

        private void OpenLogWindow()
        {
            LogView window = new LogView();
            LogViewModel = new LogViewModel();
            LogViewModel._logTarget.EventReceived += LogReceived;
            window.DataContext = LogViewModel;
            window.Show();
            window.Closed += HandleCloseLogs;
        }

        private void LogReceived(LogEventInfo obj)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                LogViewModel.Logs.Insert(0, obj);
            }));
        }

        private void UpdateViewMenu()
        {
            switch (SelectedView.Name)
            {
                case "Character":
                    Navigate(_CharacterViewModel);
                    break;
                case "Inventory":
                    Navigate(_InventoryViewModel);
                    break;
                case "Forgemagie":
                    Navigate(_MagicForgeViewModel);
                    break;
                default:
                    break;
            }
        }

        private void OpenSettingWindow()
        {
            SettingView window = new SettingView();
            SettingViewModel = new SettingViewModel(LoginConfig.Instance);
            window.DataContext = SettingViewModel;
            window.Show();
            window.Closed += HandleCloseSetting;
        }

        private void HandleCloseSetting(object sender, EventArgs e)
        {
            SettingViewModel.CanHide = true;
            RaisePropertyChanged("SettingViewModel");
        }

        private void HandleCloseLogs(object sender, EventArgs e)
        {
            LogViewModel.CanHide = true;
        }

        private bool CanProcessConnection()
        {
            return true;
        }

        private void ProcessConnection()
        {
            //DofusServer server = new DofusServer(LoginConfig.Instance.PathDofus);
            //server.Active(true, 9446);
            Proxy.Active(true, 6666);
            Proxy.Account.AccountStateUpdate += HandleStateUpdate;
        }

        private void HandleStateUpdate()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                RaisePropertyChanged(string.Empty);
            }));
        }

        public MainViewModel()
        {
            LoginConfig.Instance.Load();
            Proxy = new DofusRetroProxy(LoginConfig.Instance.PathDofus, new Account() { Username = LoginConfig.Instance.Username, Password = LoginConfig.Instance.Password });
            InteractivObject.Initialize();
            Map.Initialize();
            Spell.Initialize();
            _CharacterViewModel = new CharacterViewModel(Proxy.Account.Character.Characteristics, Proxy.Account);
            _InventoryViewModel = new InventoryViewModel(Proxy.Account);
            _MagicForgeViewModel = new MagicForgeViewModel(Proxy.Account);
            Navigate(_CharacterViewModel);
        }
    }
}
