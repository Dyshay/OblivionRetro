using NLog;
using NLog.Config;
using NLog.Targets;
using Oblivion.Utility.Logs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBoxNET.MVVM.Base;

namespace Oblivion.ViewModel.Logs
{
    public class LogViewModel : ViewModelBase
    {
        static readonly ConsoleTarget log_console = new ConsoleTarget("log_console");
        static readonly FileTarget log_file = new FileTarget("log_file") { FileName = "./log.txt" };
        static readonly LoggingConfiguration configuration = new LoggingConfiguration();
        public bool CanHide;

        public readonly MemoryEventTarget _logTarget;

        private ObservableCollection<LogEventInfo> _logs;

        public ObservableCollection<LogEventInfo> Logs
        {
            get { return _logs; }
            set { _logs = value; RaisePropertyChanged(); }
        }

        public LogViewModel()
        {
            Logs = new ObservableCollection<LogEventInfo>();
            _logTarget = new MemoryEventTarget();
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(_logTarget);
        }
    }
}
