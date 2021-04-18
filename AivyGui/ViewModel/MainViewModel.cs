using AivyData.Entities;
using AivyData.Model;
using AivyData.Model.Characters;
using AivyDofus.Model.Characters.Spells;
using AivyDofus.Protocol;
using AivyDofus.Proxy;
using AivyDofus.Server;
using AivyGui.Utilities.Log;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Interactives;
using Microsoft.Win32;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using ToolBoxNET.MVVM.Base;
using ToolBoxNET.MVVM.Command;

namespace AivyGui.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        static readonly ConsoleTarget log_console = new ConsoleTarget("log_console");
        static readonly FileTarget log_file = new FileTarget("log_file") { FileName = "./log.txt" };
        static readonly LoggingConfiguration configuration = new LoggingConfiguration();

        readonly MemoryEventTarget _logTarget;

        private int _cellId;

        public int CellId
        {
            get { return _cellId; }
            set { _cellId = value; RaisePropertyChanged(); }
        }

        public AivyData.Enums.AccountState AccountState 
        {
            get { return this.Proxy.Account.State; }
        }

        public string WindowsTitle
        {
            get { return this.Proxy.Account.Character.Nickname; }
        }


        private string _scriptPath;

        public string ScriptPath
        {
            get { return _scriptPath; }
            set { _scriptPath = value; RaisePropertyChanged(); }
        }

        private DofusRetroProxy _proxy;

        public DofusRetroProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<LogEventInfo> _logs;

        public ObservableCollection<LogEventInfo> Logs
        {
            get { return _logs; }
            set { _logs = value; RaisePropertyChanged(); }
        }

        public MainViewModel()
        {
            Proxy = new DofusRetroProxy(@"C:\Users\ABCD\AppData\Roaming\Ascalion Launcher\client\resources\app\retroclient", new Account() { Username = "oreak120", Password = "Jesuisunefleur120" });
            InteractivObject.Initialize();
            Map.Initialize();
            Spell.Initialize();
            Logs = new ObservableCollection<LogEventInfo>();
            DofusServer server = new DofusServer(@"

");
            Proxy.Account.AccountStateUpdate += HandleStateUpdate;
            Proxy.Account.Character.CharacterNameUpdate += HandleNameUpdate;
            _logTarget = new MemoryEventTarget();
            _logTarget.EventReceived += LogReceived;
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(_logTarget);
            //configuration.AddRule(LogLevel.Debug, LogLevel.Fatal, log_console);
            //LogManager.Configuration = configuration;
            //ServerEntity s_entity = server.Active(true, 779);
        }

        private void HandleNameUpdate()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                RaisePropertyChanged("WindowsTitle");
            }));
        }

        private void HandleStateUpdate()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                RaisePropertyChanged("AccountState");
            }));
        }

        private void LogReceived(LogEventInfo obj)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Logs.Insert(0, obj);
            }));
        }

        private ICommand _handleScriptLoading;

        public ICommand handleScriptLoading
        {
            get { return _handleScriptLoading ?? (_handleScriptLoading = new RelayCommand(ExecuteLoadScript)); }
        }

        private ICommand _handleConnection;

        public ICommand handleConnection
        {
            get { return _handleConnection ?? (_handleConnection = new RelayCommand(ExecuteConnection, CanExecuteConnection)); }
        }

        private ICommand _handleStartScript;

        public ICommand handleStartScript
        {
            get { return _handleStartScript ?? (_handleStartScript = new RelayCommand(ExecuteStartScript, CanExecuteStartScript)); }
        }

        private ICommand _handleStopStartScript;

        public ICommand handleStopScript
        {
            get { return _handleStopStartScript ?? (_handleStopStartScript = new RelayCommand(ExecuteStopScript, CanExecuteStopScript)); }
        }


        private bool CanExecuteStopScript()
        {
            return Proxy.Account.ScriptManager.InExecution;
        }

        private void ExecuteStopScript()
        {
            Proxy.Account.ScriptManager.Stop();
        }

        private bool CanExecuteStartScript()
        {
            return !string.IsNullOrEmpty(ScriptPath);
        }

        private void ExecuteStartScript()
        {
            Proxy.Account.ScriptManager.Start();
        }

        private bool CanExecuteConnection()
        {
            return Proxy.Account.State == AivyData.Enums.AccountState.DISCONNECTED;
        }

        private void ExecuteConnection()
        {
            Proxy.Active(true, 666);
        }

        private void ExecuteLoadScript()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Lua files (*.lua) | *.lua";
            dialog.ShowDialog();
            ScriptPath = dialog.FileName;
            if (!string.IsNullOrEmpty(ScriptPath))
                _proxy.Account.ScriptManager.LoadScript(ScriptPath);
        }
    }
}
