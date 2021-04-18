using AivyData.Model.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AivyData.Model.Scripts.Actions
{
    public interface IAction
    {
        Task ProcessAction(Account account, LuaScriptManager LuaManger);
    }
}
