using AivyData.Entities;
using AivyData.Enums;
using AivyData.Model.Characters;
using AivyData.Model.Scripts;
using AivyDofus.Model.Characters.Forge;
using AivyDomain.UseCases.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AivyData.Model
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public Character Character { get; set; }
        public int Port { get; set; }

        private AccountState _State;
        public AccountState State
        {
            get { return this._State; }
            set { this._State = value; AccountStateUpdate?.Invoke(); }
        }
        public ScriptManager ScriptManager { get; set; }
        public AivyDomain.UseCases.Client.ClientSenderRequest Sender { get; set; }
        public ClientEntity Remote { get; set; }
        public ClientEntity Client { get; set; }

        public Action AccountStateUpdate;
        private bool _IsConnected;
        public bool IsConnected
        {
            get { return _IsConnected; }
            set { _IsConnected = value; AccountStateUpdate?.Invoke(); }
        }

        public Account()
        {
            ScriptManager = new ScriptManager(this);
            Character = new Character(this);
            State = AccountState.DISCONNECTED;
        }

        public bool IsFighting() => State == AccountState.FIGHTING;
        public bool IsGathering() => State == AccountState.GATHERING;


        public void Send(string package)
        {
            this.Sender.Send(Remote, package);
        }

        public void SendFromServer(string package)
        {
            this.Sender.Send(Client, package);
        }

        public void SetSender(ClientSenderRequest client, ClientEntity receiver, ClientEntity client2)
        {
            Sender = client;
            Remote = receiver;
            Client = client2;
        }
    }
}
