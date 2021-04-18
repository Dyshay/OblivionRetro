using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Skills
{
    public class Skill
    {
        public int Id { get; set; }
        public int QuantityMax { get; set; }
        public int QuantityMin { get; set; }
        public float GatheringTime { get; set; }

        public Skill(int id, int max, int min, float time)
        {
            Id = id;
            QuantityMax = max;
            QuantityMin = min;
            GatheringTime = time;
        }
    }
}
