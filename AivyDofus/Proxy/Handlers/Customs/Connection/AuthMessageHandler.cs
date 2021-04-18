using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Protocol;
using AivyDofus.Protocol.Elements;
using AivyDofus.Proxy.Callbacks;
using AivyDomain.Callback.Client;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.Connection
{
    [ProxyHandler(ProtocolName = "HC")]
    public class AuthMessageHandler : AbstractMessageHandler
    {
        public AuthMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => false;

        public override async Task Handle()
        {
            DofusRetroProxyClientReceiveCallback _proxy_callback = _casted_callback<DofusRetroProxyClientReceiveCallback>();
            string password_key = _package.Substring(2, _package.Length - 2);
            string version = "1.33.92\n";
            string account_info = $"{_account.Username}\n{Hash.Crypt_Password(_account.Password, password_key)}\n";
            _callback._client_sender.Handle(_callback._client, Encoding.UTF8.GetBytes(version));
            _callback._client_sender.Handle(_callback._client, Encoding.UTF8.GetBytes(account_info));
            _callback._client_sender.Handle(_callback._client, Encoding.UTF8.GetBytes("Af\n"));
            _account.IsConnected = true;
            _account.State = AivyData.Enums.AccountState.CONNECTING;
            return;
        }

    }
}
