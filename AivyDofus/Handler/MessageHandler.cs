using AivyData.Entities;
using AivyDofus.Protocol.Elements;
using AivyDofus.Proxy.Callbacks;
using AivyDofus.Proxy.Handlers;
using AivyDomain.Callback.Client;
using AivyDomain.UseCases.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Handler
{
    public class MessageHandler<Attribute> where Attribute : HandlerAttribute 
    {
        protected static readonly object _lock = new object();
        protected readonly IEnumerable<Type> _handlers_type;

        public MessageHandler()
        {
            lock (_lock)
            {
                if (_handlers_type is null)
                {
                    _handlers_type = Assembly.GetExecutingAssembly()
                                             .GetTypes()
                                             .Where(x => x.GetCustomAttribute<Attribute>() != null
                                                      && x.IsSubclassOf(typeof(AbstractMessageHandler))
                                                      && !x.IsAbstract);
                }
            }
        }

        public bool Handle(AbstractClientReceiveCallback callback, string package)
        {
            return _handle(_handlers_type.FirstOrDefault(x => package.StartsWith(x.GetCustomAttribute<Attribute>().BaseMessage)), callback, package);
        }

        private bool _handle(Type handler_type, AbstractClientReceiveCallback callback, string package)
        {
            //if (!GetHandler(element.protocolID)) return true;
            if (handler_type is null) return true;

            var DofusProxyCallback = (callback as DofusRetroProxyClientReceiveCallback);

            AbstractMessageHandler handler = Activator.CreateInstance(handler_type, new object[] { callback, package, DofusProxyCallback.Account }) as AbstractMessageHandler;

            try
            {
                handler.Handle();
                handler.EndHandle();
            }
            catch (Exception e)
            {
                handler.Error(e);
            }

            return handler.IsForwardingData;
        }

        //public bool GetHandler(int protocolId)
        //{
        //    return _handlers_type.FirstOrDefault(x => x.GetCustomAttribute<Attribute>().BaseMessage.protocolID == protocolId) != null;
        //}

        public bool GetHandler(string protocolName)
        {
            return _handlers_type.FirstOrDefault(x => x.GetCustomAttribute<Attribute>().BaseMessage == protocolName) != null;
        }
    }
}
