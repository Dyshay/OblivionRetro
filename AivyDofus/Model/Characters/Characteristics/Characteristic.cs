using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Model.Characters.Characteristics
{
    public class Characteristic
    {
        public double experience_actual { get; set; }
        public double experience_min_level { get; set; }
        public double experience_max_level { get; set; }
        public int energy { get; set; }
        public int energy_max { get; set; }
        public int Health { get; set; }
        public int HealthMax { get; set; }
        public Stats Initiative { get; set; }
        public Stats Prospection { get; set; }
        public Stats PA { get; set; }
        public Stats PM { get; set; }
        public Stats Strenght { get; set; }
        public Stats Vitality { get; set; }
        public Stats Windsom { get; set; }
        public Stats Intelligence { get; set; }
        public Stats Lucky { get; set; }
        public Stats Agility { get; set; }
        public Stats PO { get; set; }
        public Stats Summon { get; set; }

        public int PercentageHealt => HealthMax == 0 ? 0 : (int)((double)Health / HealthMax * 100);

        public Characteristic()
        {
            Initiative = new Stats(0, 0, 0, 0);
            Prospection = new Stats(0, 0, 0, 0);
            PA = new Stats(0, 0, 0, 0);
            PM = new Stats(0, 0, 0, 0);
            Vitality = new Stats(0, 0, 0, 0);
            Windsom = new Stats(0, 0, 0, 0);
            Intelligence = new Stats(0, 0, 0, 0);
            Lucky = new Stats(0, 0, 0, 0);
            Agility = new Stats(0, 0, 0, 0);
            PO = new Stats(0, 0, 0, 0);
            Strenght = new Stats(0, 0, 0, 0);
            Summon = new Stats(0, 0, 0, 0);
        }

        public void UpdateCharacteristics(string package)
        {
            string[] _loc3 = package.Substring(2).Split('|');
            string[] _loc5 = _loc3[0].Split(',');

            experience_actual = double.Parse(_loc5[0]);
            experience_min_level = double.Parse(_loc5[1]);
            experience_max_level = double.Parse(_loc5[2]);

            _loc5 = _loc3[5].Split(',');
            Health = int.Parse(_loc5[0]);
            HealthMax = int.Parse(_loc5[1]);

            _loc5 = _loc3[6].Split(',');
            energy = int.Parse(_loc5[0]);
            energy_max = int.Parse(_loc5[1]);

            if (Initiative != null)
                Initiative.Base = int.Parse(_loc3[7]);
            else
                Initiative = new Stats(int.Parse(_loc3[7]));

            if (Prospection != null)
                Prospection.Base = int.Parse(_loc3[8]);
            else
                Prospection = new Stats(int.Parse(_loc3[8]));

            for (int i = 9; i <= 18; ++i)
            {
                _loc5 = _loc3[i].Split(',');
                int @base = int.Parse(_loc5[0]);
                int equipment  = int.Parse(_loc5[1]);
                int dons = int.Parse(_loc5[2]);
                int boost = int.Parse(_loc5[3]);

                switch (i)
                {
                    case 9:
                        PA.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 10:
                        PM.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 11:
                        Strenght.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 12:
                       Vitality.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 13:
                        Windsom.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 14:
                        Lucky.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 15:
                        Agility.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 16:
                        Intelligence.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 17:
                        PO.ActualizeStats(@base, equipment, dons, boost);
                        break;

                    case 18:
                        Summon.ActualizeStats(@base, equipment, dons, boost);
                        break;
                }
            }
        }
    }
}
