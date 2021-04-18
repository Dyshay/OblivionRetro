using AivyData.Model.Scripts.Actions;
using AivyData.Model.Scripts.Managers;
using AivyDofus.Model.Scripts.Actions;
using AivyDofus.Model.Scripts.Tags;
using MoonSharp.Interpreter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyData.Model.Scripts
{
    public class ScriptManager
    {
        private Account Account;
        private LuaScriptManager lua_manager;
        private ScriptState State;
        private ConcurrentQueue<IAction> ActionQueue;
        private List<ITag> Tags;
        private int Tags_Id;
        public bool IsActived { get; set; }
        public bool IsStoped { get; set; }
        public bool IsNeedScript { get; set; }
        public bool InExecution => IsActived && !IsStoped;

        public ScriptManager(Account account)
        {
            Account = account;
            lua_manager = new LuaScriptManager();
            Tags = new List<ITag>();
        }

        public void LoadScript(string path)
        {
            if (IsActived)
                IsStoped = true;

            if (!File.Exists(path) || !path.EndsWith(".lua"))
                return;

            lua_manager.LoadFromFile(path);
        }

        public void Start(bool needScript = true)
        {
            if (IsActived || Account.State == Enums.AccountState.DISCONNECTED)
                return;

            IsActived = true;
            IsStoped = false;

            IsNeedScript = needScript;
            ActionQueue = new ConcurrentQueue<IAction>();

            State = ScriptState.MOVE;

            InitializeScript();
        }

        public void Stop()
        {
            if (InExecution || Account.State != Enums.AccountState.DISCONNECTED)
            {
                IsActived = false;
                IsStoped = true;
            }
        }

        private void InitializeScript() => Task.Run(async () =>
        {
            if (!InExecution)
                return;

            await InitializeCheck();

            if (IsNeedScript)
            {
                ActionQueue = new ConcurrentQueue<IAction>();
                List<Table> Entries = lua_manager.GetFunction(State).ToList();

                if (Entries == null)
                    return;

                if (Account.State != Enums.AccountState.FIGHTING)
                {
                    foreach (Table entrie in Entries)
                    {
                        if (entrie["map"] == null)
                            continue;

                        if (!Account.Character.IsOnMap(entrie["map"].ToString()))
                            continue;

                        Process(entrie);
                        ProcessTags();
                        break;
                    }

                    var delayHumain = new Random().Next(1, 6500);
                    await Task.Delay(delayHumain);

                    await ProcessDequeueAction();
                }
            }
            if (Account.State == Enums.AccountState.IDLE && !IsNeedScript)
            {
                var delayHumain = new Random().Next(1, 300);
                await Task.Delay(delayHumain);
                if (ActionQueue.Count > 0)
                {
                    await ProcessDequeueAction();
                }
            }



            InitializeScript();
        });

        public void AddAction(IAction action)
        {
            if (ActionQueue == null)
                ActionQueue = new ConcurrentQueue<IAction>();
            ActionQueue.Enqueue(action);
        }

        private async Task ProcessDequeueAction()
        {
            if (ActionQueue.TryDequeue(out IAction action))
            {
                await ProcessAction(action);
                await ProcessDequeueAction();
            }
        }

        private void Process(Table entrie)
        {
            Tags.Clear();
            Tags_Id = 0;


            DynValue action = null;
            switch (State)
            {
                case ScriptState.MOVE:
                    action = entrie.Get("fight");
                    if (!action.IsNil() && action.Type == DataType.Boolean && action.Boolean)
                        Tags.Add(new FightTag());
                    break;
                case ScriptState.BANK:
                    break;
                case ScriptState.PHENIX:
                    break;
                default:
                    break;
            }

            action = entrie.Get("cell");
            if (!action.IsNil() && action.Type == DataType.Number)
                Tags.Add(new ChangeMapTag((int)action.Number));

            //TODO Return error with no script on this map
            if (Tags.Count == 0)
                return;
        }

        private void ProcessTags()
        {
            if (!InExecution)
                return;

            switch (Tags[Tags_Id])
            {
                case FightTag _:
                    ActionQueue.Enqueue(new FightAction());
                    break;
                case ChangeMapTag tag:
                    ActionQueue.Enqueue(new ChangeMapAction(tag));
                    break;
                default:
                    break;
            }

            //if (Tags_Id + 1 <= Tags.Count)
            //{
            //    Tags_Id++;
            //    ProcessTags();
            //}
        }

        private async Task ProcessAction(IAction action)
        {
            await action.ProcessAction(Account, lua_manager);
        }

        private Task InitializeCheck()
        {
            return Task.Delay(500);
        }
    }
}
