using EasyHook;
using NLog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace SocketHook
{
    public class HookManager
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static HookElement CreateElement<Interface>(Interface ipcInterface) where Interface : HookInterface  
        {
            HookElement element = new HookElement();
            {
                element.IpcServer = RemoteHooking.IpcCreateServer(ref element.ChannelName, WellKnownObjectMode.Singleton, ipcInterface);
            }
            return element;
        }

        public static void Log(string message)
        {
            logger.Info(message);
        }
    }
}
