using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Effects
{
    public class Effect
    {
        public EffectEnum Id { get; set; }
        public int Stats { get; set; }

        public Effect(EffectEnum id, string stats)
        {
            Id = id;
            if(int.TryParse(stats,out int data))
            {
                Stats = data;
            }
            
        }
    }
}
