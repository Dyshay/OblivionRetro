using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Characteristics
{
    public class Stats
    {
        public int Base { get; set; }
        public int Equipment { get; set; }
        public int Dons { get; set; }
        public int Boost { get; set; }
        public int Total => Base + Equipment + Dons + Boost;

        public Stats(int @base)
        {
            Base = @base;
        }

        public Stats(int @base, int equipment, int dons, int boost) => ActualizeStats(@base, equipment, dons, boost);

        public void ActualizeStats(int @base, int equipment, int dons, int boost)
        {
            Base = @base;
            Equipment = equipment;
            Dons = dons;
            Boost = boost;
        }
    }
}
