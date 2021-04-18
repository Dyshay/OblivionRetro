using AivyDofus.Model.Characters.Skills;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DeepBot.Model.Account.Game.Maps.Interactives
{
    public enum InteractiveObjectIdEnum
    {
        INTERACTIVE_ALEMBIC = 62,
        INTERACTIVE_ORME = 30,
        INTERACTIVE_ERABLE = 31,
        INTERACTIVE_PLAN_DE_TRAVAIL = 95,
        INTERACTIVE_DOLOMITE = 113,
        INTERACTIVE_MACHINE_A_COUDRE = 86,
        INTERACTIVE_TABLE_MAGIQUE = 118,
        INTERACTIVE_CHARME = 32,
        INTERACTIVE_NOYER = 34,
        INTERACTIVE_PLAN_DE_TRAVAIL_1 = 96,
        INTERACTIVE_MARMITE = 60,
        INTERACTIVE_COFFRE = 85,
        INTERACTIVE_ATELIER_MAGIQUE = 117,
        INTERACTIVE_EDELWEISS = 61,
        INTERACTIVE_FRENE = 1,
        INTERACTIVE_EBENE = 29,
        INTERACTIVE_ATELIER_DE_BRICOLAGE = 122,
        INTERACTIVE_PICHON = 100,
        INTERACTIVE_ORCHIDEE_FREYESQUE = 68,
        INTERACTIVE_LISTE_DES_ARTISANS = 119,
        INTERACTIVE_MACHINE_A_COUDRE_1 = 58,
        INTERACTIVE_COTON = 82,
        INTERACTIVE_FROMENT = 63,
        INTERACTIVE_POISSONS_GEANTS_MER = 81,
        INTERACTIVE_MOULE = 27,
        INTERACTIVE_EPEAUTRE = 64,
        INTERACTIVE_SCIE = 2,
        INTERACTIVE_IF = 28,
        INTERACTIVE_BLE = 38,
        INTERACTIVE_KALIPTUS = 121,
        INTERACTIVE_ENCLUME = 57,
        INTERACTIVE_ENCLOS = 120,
        INTERACTIVE_PLAN_DE_TRAVAIL_2 = 94,
        INTERACTIVE_MERISIER = 35,
        INTERACTIVE_MANGANESE = 54,
        INTERACTIVE_POUBELLE = 105,
        INTERACTIVE_PIERRE_CUIVREE = 53,
        INTERACTIVE_SORGHO = 65,
        INTERACTIVE_CHATAIGNIER = 33,
        INTERACTIVE_POISSON_DE_FRIGOST = 132,
        INTERACTIVE_MACHINE_A_COUDRE_MAGIQUE = 116,
        INTERACTIVE_CONCASSEUR_MUNSTER = 93,
        INTERACTIVE_ARGENT = 24,
        INTERACTIVE_ETABLI_EN_BOIS = 88,
        INTERACTIVE_BRONZE = 55,
        INTERACTIVE_PIERRE_DE_BAUXITE = 26,
        INTERACTIVE_SILICATE = 114,
        INTERACTIVE_LEVIER = 127,
        INTERACTIVE_TREFLE_A_5_FEUILLES = 67,
        INTERACTIVE_BAMBOU = 108,
        INTERACTIVE_GROS_POISSONS_RIVIERE = 76,
        INTERACTIVE_AVOINE = 45,
        INTERACTIVE_STATUE_DE_CLASSE = 128,
        INTERACTIVE_POISSONS_RIVIERE = 74,
        INTERACTIVE_CHANVRE = 46,
        INTERACTIVE_OR = 25,
        INTERACTIVE_SOURCE_DE_JOUVENCE = 56,
        INTERACTIVE_CHAUDRON = 15,
        INTERACTIVE_POISSONS_GEANTS_RIVIERE = 79,
        INTERACTIVE_ENCLUME_A_BOUCLIERS = 107,
        INTERACTIVE_ORGE = 43,
        INTERACTIVE_ZAAP = 16,
        INTERACTIVE_TAS_DE_PATATES = 48,
        INTERACTIVE_PANDOUILLE = 112,
        INTERACTIVE_TRUITE_VASEUSE = 80,
        INTERACTIVE_PWOULPE = 73,
        INTERACTIVE_MEULE = 41,
        INTERACTIVE_PLAN_DE_TRAVAIL_3 = 97,
        INTERACTIVE_PORTE = 70,
        INTERACTIVE_ZAAPI = 106,
        INTERACTIVE_FILEUSE = 83,
        INTERACTIVE_PERCE_NEIGE = 131,
        INTERACTIVE_MALT = 47,
        INTERACTIVE_PETITS_POISSONS_RIVIERE = 75,
        INTERACTIVE_MENTHE_SAUVAGE = 66,
        INTERACTIVE_ETABLI_PYROTECHNIQUE = 103,
        INTERACTIVE_BOMBU = 98,
        INTERACTIVE_ATELIER = 12,
        INTERACTIVE_SEIGLE = 44,
        INTERACTIVE_FER = 17,
        INTERACTIVE_ETABLI = 13,
        INTERACTIVE_POISSONS_MER = 77,
        INTERACTIVE_MACHINE_DE_FORCE = 102,
        INTERACTIVE_PUITS = 84,
        INTERACTIVE_PIERRE_DE_KOBALTE = 37,
        INTERACTIVE_OLIVIOLET = 101,
        INTERACTIVE_CHENE = 8,
        INTERACTIVE_MOULIN = 40,
        INTERACTIVE_OMBRE_ETRANGE = 99,
        INTERACTIVE_KOINKOIN = 104,
        INTERACTIVE_SOMOON_AGRESSIF = 72,
        INTERACTIVE_OBSIDIENNE = 135,
        INTERACTIVE_TABLE_A_PATATES = 49,
        INTERACTIVE_PETITS_POISSONS_MER = 71,
        INTERACTIVE_FOUR = 22,
        INTERACTIVE_FROSTIZZ = 134,
        INTERACTIVE_GROS_POISSONS_MER = 78,
        INTERACTIVE_ETAIN = 52,
        INTERACTIVE_LIN = 42,
        INTERACTIVE_ALAMBIC = 90,
        INTERACTIVE_RIZ = 111,
        INTERACTIVE_TABLE_DE_CONFECTION = 11,
        INTERACTIVE_CONCASSEUR = 50,
        INTERACTIVE_HOUBLON = 39,
        INTERACTIVE_BAMBOU_SOMBRE = 109,
        INTERACTIVE_ENCLUME_MAGIQUE = 92,
        INTERACTIVE_TREMBLE = 133,
        INTERACTIVE_BAMBOU_SACRE = 110,
        INTERACTIVE_MORTIER_ET_PILON = 69,
        INTERACTIVE_PHOENIX = 200,
    }
    public class InteractivObject
    {
        public static Dictionary<int, InteractivObject> InteractivesObjects { get; set; }

        public int Id { get; set; }
        public List<int> IdSkill { get; set; } = new List<int>();
        public string Name { get; set; }
        public bool IsUsable { get; set; } = true;

        public static void Initialize()
        {
            InteractivesObjects = new Dictionary<int, InteractivObject>();
            var dll = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Interactives.json"))
            {
                string result = reader.ReadToEnd();
                InteractivesObjects = JsonConvert.DeserializeObject<List<InteractivObject>>(result).ToDictionary(c => c.Id, c => c);
            }

            GenerateInteractiveByGfx(7500, InteractiveObjectIdEnum.INTERACTIVE_FRENE, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_FRENE });
            GenerateInteractiveByGfx(7003, InteractiveObjectIdEnum.INTERACTIVE_SCIE, new SkillIdEnum[] { SkillIdEnum.SKILL_SCIER });
            GenerateInteractiveByGfx(7503, InteractiveObjectIdEnum.INTERACTIVE_CHENE, new SkillIdEnum[] { });
            GenerateInteractiveByGfx(7011, InteractiveObjectIdEnum.INTERACTIVE_TABLE_DE_CONFECTION, new SkillIdEnum[] { SkillIdEnum.SKILL_CONFECTIONNER_DES_BOTTES, SkillIdEnum.SKILL_CONFECTIONNER_UNE_CEINTURE });
            GenerateInteractiveByGfx(7008, InteractiveObjectIdEnum.INTERACTIVE_ATELIER);
            GenerateInteractiveByGfx(7009, InteractiveObjectIdEnum.INTERACTIVE_ATELIER);
            GenerateInteractiveByGfx(7010, InteractiveObjectIdEnum.INTERACTIVE_ATELIER);
            GenerateInteractiveByGfx(7013, InteractiveObjectIdEnum.INTERACTIVE_ETABLI, new SkillIdEnum[] { SkillIdEnum.SKILL_UTILISER_ETABLI });
            GenerateInteractiveByGfx(7000, InteractiveObjectIdEnum.INTERACTIVE_ZAAP, new SkillIdEnum[] { SkillIdEnum.SKILL_UTILISER_ZAAP });
            GenerateInteractiveByGfx(7026, InteractiveObjectIdEnum.INTERACTIVE_ZAAP, new SkillIdEnum[] { SkillIdEnum.SKILL_UTILISER_ZAAP });
            GenerateInteractiveByGfx(7029, InteractiveObjectIdEnum.INTERACTIVE_ZAAP, new SkillIdEnum[] { SkillIdEnum.SKILL_UTILISER_ZAAP });
            GenerateInteractiveByGfx(4287, InteractiveObjectIdEnum.INTERACTIVE_ZAAP, new SkillIdEnum[] { SkillIdEnum.SKILL_UTILISER_ZAAP });
            GenerateInteractiveByGfx(7520, InteractiveObjectIdEnum.INTERACTIVE_FER, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_FER });
            GenerateInteractiveByGfx(7001, InteractiveObjectIdEnum.INTERACTIVE_FOUR);
            GenerateInteractiveByGfx(7526, InteractiveObjectIdEnum.INTERACTIVE_ARGENT, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_ARGENT });
            GenerateInteractiveByGfx(7527, InteractiveObjectIdEnum.INTERACTIVE_OR, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_OR });
            GenerateInteractiveByGfx(7528, InteractiveObjectIdEnum.INTERACTIVE_PIERRE_DE_BAUXITE, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_BAUXITE });
            GenerateInteractiveByGfx(7002, InteractiveObjectIdEnum.INTERACTIVE_MOULE);
            GenerateInteractiveByGfx(7505, InteractiveObjectIdEnum.INTERACTIVE_IF, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_IF });
            GenerateInteractiveByGfx(7507, InteractiveObjectIdEnum.INTERACTIVE_EBENE, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_EBENE });
            GenerateInteractiveByGfx(7509, InteractiveObjectIdEnum.INTERACTIVE_ORME, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_ORME });
            GenerateInteractiveByGfx(7504, InteractiveObjectIdEnum.INTERACTIVE_ERABLE, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_ERABLE });
            GenerateInteractiveByGfx(7508, InteractiveObjectIdEnum.INTERACTIVE_CHARME, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_CHARME });
            GenerateInteractiveByGfx(7501, InteractiveObjectIdEnum.INTERACTIVE_CHATAIGNIER, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_CHATAIGNER });
            GenerateInteractiveByGfx(7502, InteractiveObjectIdEnum.INTERACTIVE_NOYER, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_NOYER });
            GenerateInteractiveByGfx(7506, InteractiveObjectIdEnum.INTERACTIVE_MERISIER, new SkillIdEnum[] { SkillIdEnum.SKILL_COUPER_MERISIER });
            GenerateInteractiveByGfx(7525, InteractiveObjectIdEnum.INTERACTIVE_PIERRE_DE_KOBALTE, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_KOBALTE });
            GenerateInteractiveByGfx(7511, InteractiveObjectIdEnum.INTERACTIVE_BLE, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_BLE });
            GenerateInteractiveByGfx(7512, InteractiveObjectIdEnum.INTERACTIVE_HOUBLON, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_HOUBLON });
            GenerateInteractiveByGfx(7005, InteractiveObjectIdEnum.INTERACTIVE_MEULE);
            GenerateInteractiveByGfx(7513, InteractiveObjectIdEnum.INTERACTIVE_LIN, new SkillIdEnum[] { SkillIdEnum.SKILL_CUEILLIR_LIN, SkillIdEnum.SKILL_FAUCHER_LIN });
            GenerateInteractiveByGfx(7515, InteractiveObjectIdEnum.INTERACTIVE_ORGE, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_ORGE });
            GenerateInteractiveByGfx(7516, InteractiveObjectIdEnum.INTERACTIVE_SEIGLE, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_SEIGLE });
            GenerateInteractiveByGfx(7517, InteractiveObjectIdEnum.INTERACTIVE_AVOINE, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_AVOINE });
            GenerateInteractiveByGfx(7514, InteractiveObjectIdEnum.INTERACTIVE_CHANVRE, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_CHANVRE, SkillIdEnum.SKILL_CUEILLIR_CHANVRE });
            GenerateInteractiveByGfx(7518, InteractiveObjectIdEnum.INTERACTIVE_MALT, new SkillIdEnum[] { SkillIdEnum.SKILL_FAUCHER_MALT });
            GenerateInteractiveByGfx(7510, InteractiveObjectIdEnum.INTERACTIVE_TAS_DE_PATATES);
            GenerateInteractiveByGfx(7006, InteractiveObjectIdEnum.INTERACTIVE_TABLE_A_PATATES);
            GenerateInteractiveByGfx(7007, InteractiveObjectIdEnum.INTERACTIVE_CONCASSEUR);
            GenerateInteractiveByGfx(7521, InteractiveObjectIdEnum.INTERACTIVE_ETAIN, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_ETAIN });
            GenerateInteractiveByGfx(7522, InteractiveObjectIdEnum.INTERACTIVE_PIERRE_CUIVREE, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_CUIVRE });
            GenerateInteractiveByGfx(7524, InteractiveObjectIdEnum.INTERACTIVE_MANGANESE, new SkillIdEnum[] { SkillIdEnum.SKILL_COLLECTER_MANGANESE });
            GenerateInteractiveByGfx(7523, InteractiveObjectIdEnum.INTERACTIVE_BRONZE);
            GenerateInteractiveByGfx(7004, InteractiveObjectIdEnum.INTERACTIVE_SOURCE_DE_JOUVENCE);
            GenerateInteractiveByGfx(7012, InteractiveObjectIdEnum.INTERACTIVE_ENCLUME);
            GenerateInteractiveByGfx(7014, InteractiveObjectIdEnum.INTERACTIVE_MACHINE_A_COUDRE_1);
            GenerateInteractiveByGfx(7016, InteractiveObjectIdEnum.INTERACTIVE_MACHINE_A_COUDRE_1);
            GenerateInteractiveByGfx(7017, InteractiveObjectIdEnum.INTERACTIVE_MARMITE);
            GenerateInteractiveByGfx(7536, InteractiveObjectIdEnum.INTERACTIVE_EDELWEISS);
            GenerateInteractiveByGfx(7534, InteractiveObjectIdEnum.INTERACTIVE_MENTHE_SAUVAGE);
            GenerateInteractiveByGfx(7533, InteractiveObjectIdEnum.INTERACTIVE_TREFLE_A_5_FEUILLES);
            GenerateInteractiveByGfx(7535, InteractiveObjectIdEnum.INTERACTIVE_ORCHIDEE_FREYESQUE);
            GenerateInteractiveByGfx(6700, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6701, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6702, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6703, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6704, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6705, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6706, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6707, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6708, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6709, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6710, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6711, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6713, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6714, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6715, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6716, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6717, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6718, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6719, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6720, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6721, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6722, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6723, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6724, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6725, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6726, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6729, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6730, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6731, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6732, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6733, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6734, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6735, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6736, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6737, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6738, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6739, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6740, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6741, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6742, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6743, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6744, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6745, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6746, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6747, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6748, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6753, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6749, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6750, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6751, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6752, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6754, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6756, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6755, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6757, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6758, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6764, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6765, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6768, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6759, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6760, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6761, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6762, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6769, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6770, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(4516, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6773, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6774, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6775, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(6776, InteractiveObjectIdEnum.INTERACTIVE_PORTE);
            GenerateInteractiveByGfx(7530, InteractiveObjectIdEnum.INTERACTIVE_PETITS_POISSONS_MER);
            GenerateInteractiveByGfx(7532, InteractiveObjectIdEnum.INTERACTIVE_POISSONS_RIVIERE);
            GenerateInteractiveByGfx(7529, InteractiveObjectIdEnum.INTERACTIVE_PETITS_POISSONS_RIVIERE);
            GenerateInteractiveByGfx(7537, InteractiveObjectIdEnum.INTERACTIVE_GROS_POISSONS_RIVIERE);
            GenerateInteractiveByGfx(7531, InteractiveObjectIdEnum.INTERACTIVE_POISSONS_MER);
            GenerateInteractiveByGfx(7538, InteractiveObjectIdEnum.INTERACTIVE_GROS_POISSONS_MER);
            GenerateInteractiveByGfx(7539, InteractiveObjectIdEnum.INTERACTIVE_POISSONS_GEANTS_RIVIERE);
            GenerateInteractiveByGfx(7540, InteractiveObjectIdEnum.INTERACTIVE_POISSONS_GEANTS_MER);
            GenerateInteractiveByGfx(7519, InteractiveObjectIdEnum.INTERACTIVE_PUITS);
            GenerateInteractiveByGfx(7350, InteractiveObjectIdEnum.INTERACTIVE_COFFRE);
            GenerateInteractiveByGfx(7351, InteractiveObjectIdEnum.INTERACTIVE_COFFRE);
            //GenerateInteractiveByGfx(7352, InteractiveObjectIdEnum.INTERACTIVE_COFFRE);
            GenerateInteractiveByGfx(7353, InteractiveObjectIdEnum.INTERACTIVE_COFFRE);
            GenerateInteractiveByGfx(7015, InteractiveObjectIdEnum.INTERACTIVE_MACHINE_A_COUDRE);
            GenerateInteractiveByGfx(7018, InteractiveObjectIdEnum.INTERACTIVE_ETABLI_EN_BOIS);
            GenerateInteractiveByGfx(7019, InteractiveObjectIdEnum.INTERACTIVE_ALAMBIC);
            GenerateInteractiveByGfx(7020, InteractiveObjectIdEnum.INTERACTIVE_ENCLUME_MAGIQUE);
            GenerateInteractiveByGfx(7021, InteractiveObjectIdEnum.INTERACTIVE_CONCASSEUR_MUNSTER);
            GenerateInteractiveByGfx(7022, InteractiveObjectIdEnum.INTERACTIVE_PLAN_DE_TRAVAIL_2);
            GenerateInteractiveByGfx(7025, InteractiveObjectIdEnum.INTERACTIVE_PLAN_DE_TRAVAIL);
            GenerateInteractiveByGfx(7024, InteractiveObjectIdEnum.INTERACTIVE_PLAN_DE_TRAVAIL_1);
            GenerateInteractiveByGfx(7023, InteractiveObjectIdEnum.INTERACTIVE_PLAN_DE_TRAVAIL_3);
            GenerateInteractiveByGfx(7541, InteractiveObjectIdEnum.INTERACTIVE_BOMBU);
            GenerateInteractiveByGfx(7543, InteractiveObjectIdEnum.INTERACTIVE_OMBRE_ETRANGE);
            GenerateInteractiveByGfx(7544, InteractiveObjectIdEnum.INTERACTIVE_PICHON);
            GenerateInteractiveByGfx(7542, InteractiveObjectIdEnum.INTERACTIVE_OLIVIOLET);
            GenerateInteractiveByGfx(7546, InteractiveObjectIdEnum.INTERACTIVE_MACHINE_DE_FORCE);
            GenerateInteractiveByGfx(7547, InteractiveObjectIdEnum.INTERACTIVE_MACHINE_DE_FORCE);
            GenerateInteractiveByGfx(7028, InteractiveObjectIdEnum.INTERACTIVE_ETABLI_PYROTECHNIQUE);
            GenerateInteractiveByGfx(7549, InteractiveObjectIdEnum.INTERACTIVE_KOINKOIN);
            GenerateInteractiveByGfx(7352, InteractiveObjectIdEnum.INTERACTIVE_POUBELLE);
            GenerateInteractiveByGfx(7030, InteractiveObjectIdEnum.INTERACTIVE_ZAAPI);
            GenerateInteractiveByGfx(7031, InteractiveObjectIdEnum.INTERACTIVE_ZAAPI);
            GenerateInteractiveByGfx(7027, InteractiveObjectIdEnum.INTERACTIVE_ENCLUME_A_BOUCLIERS);
            GenerateInteractiveByGfx(7553, InteractiveObjectIdEnum.INTERACTIVE_BAMBOU);
            GenerateInteractiveByGfx(7554, InteractiveObjectIdEnum.INTERACTIVE_BAMBOU_SOMBRE);
            GenerateInteractiveByGfx(7552, InteractiveObjectIdEnum.INTERACTIVE_BAMBOU_SACRE);
            GenerateInteractiveByGfx(7550, InteractiveObjectIdEnum.INTERACTIVE_RIZ);
            GenerateInteractiveByGfx(7551, InteractiveObjectIdEnum.INTERACTIVE_PANDOUILLE);
            GenerateInteractiveByGfx(7555, InteractiveObjectIdEnum.INTERACTIVE_DOLOMITE);
            GenerateInteractiveByGfx(7556, InteractiveObjectIdEnum.INTERACTIVE_SILICATE);
            GenerateInteractiveByGfx(7036, InteractiveObjectIdEnum.INTERACTIVE_MACHINE_A_COUDRE_MAGIQUE);
            GenerateInteractiveByGfx(7038, InteractiveObjectIdEnum.INTERACTIVE_ATELIER_MAGIQUE, new SkillIdEnum[] { SkillIdEnum.SKILL_AMELIORER_UN_ANNEAU, SkillIdEnum.SKILL_AMELIORER_UNE_AMULETTE });
            GenerateInteractiveByGfx(7037, InteractiveObjectIdEnum.INTERACTIVE_TABLE_MAGIQUE);
            GenerateInteractiveByGfx(7035, InteractiveObjectIdEnum.INTERACTIVE_LISTE_DES_ARTISANS);
            GenerateInteractiveByGfx(6766, InteractiveObjectIdEnum.INTERACTIVE_ENCLOS);
            GenerateInteractiveByGfx(6767, InteractiveObjectIdEnum.INTERACTIVE_ENCLOS);
            GenerateInteractiveByGfx(6763, InteractiveObjectIdEnum.INTERACTIVE_ENCLOS);
            GenerateInteractiveByGfx(6772, InteractiveObjectIdEnum.INTERACTIVE_ENCLOS);
            GenerateInteractiveByGfx(7557, InteractiveObjectIdEnum.INTERACTIVE_KALIPTUS);
            GenerateInteractiveByGfx(7039, InteractiveObjectIdEnum.INTERACTIVE_ATELIER_DE_BRICOLAGE);
            GenerateInteractiveByGfx(7041, InteractiveObjectIdEnum.INTERACTIVE_LEVIER);
            GenerateInteractiveByGfx(7042, InteractiveObjectIdEnum.INTERACTIVE_LEVIER);
            GenerateInteractiveByGfx(7043, InteractiveObjectIdEnum.INTERACTIVE_LEVIER);
            GenerateInteractiveByGfx(7044, InteractiveObjectIdEnum.INTERACTIVE_LEVIER);
            GenerateInteractiveByGfx(7045, InteractiveObjectIdEnum.INTERACTIVE_LEVIER);
            GenerateInteractiveByGfx(1853, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1854, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1855, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1856, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1857, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1858, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1859, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1860, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1861, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1862, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(1845, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);
            GenerateInteractiveByGfx(2319, InteractiveObjectIdEnum.INTERACTIVE_STATUE_DE_CLASSE);

            GenerateInteractiveByGfx(4638, InteractiveObjectIdEnum.INTERACTIVE_PHOENIX);
        }

        public static void GenerateInteractiveByGfx(int id, InteractiveObjectIdEnum type, SkillIdEnum[] skills = null)
        {
            if (InteractivesObjects.ContainsKey(id))
            {
                if (skills != null)
                    InteractivesObjects[id].IdSkill = new List<int>(skills.ToList().Select(c => (int)c));
                else
                    InteractivesObjects[id].IdSkill = new List<int>();
            }
        }

        public InteractivObject Clone()
        {
            return new InteractivObject()
            {
                Id = Id,
                Name = Name,
                IsUsable = IsUsable,
            };
        }
    }
}
