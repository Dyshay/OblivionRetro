using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Pattern
{
    public class Singleton<T> where T : new()
    {
        private static T _Instance;
        public static T Instance
        {
            get { return _Instance ?? (_Instance = new T()); }
        }
    }
}
