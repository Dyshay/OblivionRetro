using AivyData.Entities;
using AivyData.Enums;
using AivyData.Model;
using AivyDofus.Protocol;
using AivyDomain.Callback.Client;
using AivyDomain.UseCases.Client;
using NLog;
using Org.Mentalis.Network.ProxySocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Callbacks
{
    public class DofusRetroProxyAcceptCallback : DofusProxyAcceptCallback
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Account Account { get; set; }

        public DofusRetroProxyAcceptCallback(ProxyEntity proxy,Account account)
            : base(proxy)
        {
            Account = account;
        }

        public override void Callback(IAsyncResult result)
        {
            if (_proxy.IsRunning)
            {
                _proxy.Socket = (ProxySocket)result.AsyncState;

                Socket _client_socket = _proxy.Socket.EndAccept(result);

                ClientEntity client = _client_creator.Handle(_client_socket.RemoteEndPoint as IPEndPoint);
                client = _client_linker.Handle(client, _client_socket);

                ClientEntity remote = _client_creator.Handle(_proxy.IpRedirectedStack.Dequeue());
                ProxySocket Socket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.ProxyEndPoint = new IPEndPoint(IPAddress.Parse("109.248.110.177"), 24532);
                Socket.ProxyUser = "nemulg";
                Socket.ProxyPass = "Dz4RNr2Ipk";
                Socket.ProxyType = ProxyTypes.Socks5;
                remote = _client_linker.Handle(remote, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
                DofusRetroProxyClientReceiveCallback remote_rcv_callback = new DofusRetroProxyClientReceiveCallback(remote, client, _client_repository, _client_creator, _client_linker, _client_connector, _client_disconnector, _client_sender, _proxy, Account, ProxyTagEnum.Server);
                remote = _client_connector.Handle(remote, new ClientConnectCallback(remote, remote_rcv_callback));

                //ProxyManager.Instance.Clients[_proxy.Port].Client = _client_sender;
                //ProxyManager.Instance.Clients[_proxy.Port].Proxy = _proxy;
                //ProxyManager.Instance.Clients[_proxy.Port].ClientDofus = client;
                //ProxyManager.Instance.Clients[_proxy.Port].Remote = remote;

                //Client = _client_sender;
                //ClientDofus = client;
                //RemoteServer = remote;
                Account.SetSender(_client_sender, remote, client);

                // wait remote client to connect
                try
                {
                    while (!remote.IsRunning) ;
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    return;
                }

                if (client.IsRunning)
                {
                    client = _client_receiver.Handle(client, new DofusRetroProxyClientReceiveCallback(client, remote, _client_repository, _client_creator, _client_linker, _client_connector, _client_disconnector, _client_sender, _proxy, Account, ProxyTagEnum.Client));

                    logger.Info("client connected");
                }

                _proxy.Socket.BeginAccept(Callback, _proxy.Socket);
            }
        }
    }
}
