using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Model.Scripts.Actions;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs
{
    [ProxyHandler(ProtocolName = "cMK#")]
    public class ChatMessageHandler : AbstractMessageHandler
    {
        public ChatMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => false;

        public override async Task Handle()
        {
            string[] data = _package.Split('|');
            switch (data[3])
            {
                case string x when x.StartsWith("click"):
                    _account.Character.IsAutoClick = !_account.Character.IsAutoClick;
                    break;
                case string x when x.StartsWith("skill"):
                    if (_account.ScriptManager.IsActived)
                    {
                        _account.ScriptManager.Stop();
                        _account.SendFromServer($"cMK@|{_account.Character.Id}|[SispaBot]|Metier désactivé");
                    }
                    else
                    {
                        _account.ScriptManager.Start(false);
                        _account.SendFromServer($"cMK@|{_account.Character.Id}|[SispaBot]|Metier activé");
                        foreach (var item in _account.Character.Map.Interactives)
                        {
                            if (item.Value.IsUsable)
                            {
                                _account.ScriptManager.AddAction(new GatheringAction(item.Key));
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
