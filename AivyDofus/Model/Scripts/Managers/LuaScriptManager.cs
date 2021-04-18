using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AivyData.Model.Scripts.Managers
{
    public class LuaScriptManager
    {
        public Script Script { get; private set; }
        public void LoadFromFile(string path_script)
        {
            Script = new Script();

            Script.DoFile(path_script);
        }

        public IEnumerable<Table> GetFunction(ScriptState function_name)
        {
            DynValue function = Script.Globals.Get(function_name.ToString());

            if (function.IsNil() || function.Type != DataType.Function)
                return null;

            DynValue result = Script.Call(function);

            return result.Type != DataType.Table ? null : result.Table.Values.Where(data => data.Type == DataType.Table).Select(f => f.Table);
        }

        public T Get<T>(string key, DataType type, T value)
        {
            DynValue global = Script.Globals.Get(key);

            if (global.IsNil() || global.Type != type)
                return value;

            try
            {
                return (T)global.ToObject(typeof(T));
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}
