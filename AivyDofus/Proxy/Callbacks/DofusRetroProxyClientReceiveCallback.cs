using AivyData.Entities;
using AivyData.Enums;
using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDofus.Proxy.Handlers;
using AivyDomain.Repository.Client;
using AivyDomain.UseCases.Client;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Callbacks
{
    public class DofusRetroProxyClientReceiveCallback : DofusProxyClientReceiveCallback
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static int Port { get; set; }
        //protected MessageHandler<ProxyHandlerAttribute> _handler;
        public Account Account { get; set; }
        private string PacketBuffer;

        public DofusRetroProxyClientReceiveCallback(ClientEntity client, ClientEntity remote, ClientRepository repository, ClientCreatorRequest creator, ClientLinkerRequest linker, ClientConnectorRequest connector, ClientDisconnectorRequest disconnector, ClientSenderRequest sender, ProxyEntity proxy, Account account, ProxyTagEnum tag = ProxyTagEnum.UNKNOW)
            : base(client, remote, repository, creator, linker, connector, disconnector, sender, proxy, tag)
        {
            Account = account;
            Account.Port = _proxy.Port;

            _handler = new MessageHandler<ProxyHandlerAttribute>();
            _rcv_action += OnReceive;
            //_rcv_action += Received;
        }


        /// <summary>
        /// to do , packet reading , this is just a test
        /// </summary>
        /// <param name="stream"></param>
        protected override void OnReceive(MemoryStream stream)
        {
            byte[] arr = stream.ToArray();
            string msg = Encoding.UTF8.GetString(arr);
            var packages = msg.Replace("\x0a", string.Empty).Split('\0').Where(x => x != string.Empty).ToList();
            foreach (var packet in packages)
            {
                if (packages.IndexOf(packet) == packages.Count - 1 && !msg.EndsWith("\0"))
                {
                    this.PacketBuffer = packet;
                }
                else if (!String.IsNullOrEmpty(this.PacketBuffer))
                {
                    if (!_handler.Handle(this, PacketBuffer + packet))
                    {
                        this.PacketBuffer = null;
                        break;
                    }
                    this.PacketBuffer = null;
                }
                else
                {
                    logger.Info($"[{_tag}] : {packet}");
                    if (!_handler.Handle(this, packet))
                    {
                        break;
                    }
                }

            }


            _client_sender.Handle(_remote, Encoding.UTF8.GetBytes(msg));
        }
    }
}
