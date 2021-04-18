using AivyData.Enums.Map;
using AivyDofus.Model.Maps;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Entities;
using DeepBot.Model.Account.Game.Maps.Interactives;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace DeepBot.Model.Account.Game.Maps
{
    public class Map
    {
        public static Dictionary<int, Map> Maps { get; set; }

        public string GlobalAreaName { get; set; }
        public string AreaName { get; set; }
        public int AreaId { get; set; }
        public int MapId { get; set; }
        public string MapData { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Coordinate { get; set; }
        public Cell[] Cells { get; set; }
        public ConcurrentDictionary<MovementDirection, List<short>> CellsTeleport;
        public ConcurrentDictionary<int, Entity> Entities;
        public ConcurrentDictionary<int, InteractivObject> Interactives;

        public event Action EntitiesUpdate;
        public event Action<int, string> EntityMoved;
        public Pathmaker Pathmaker;

        public Map()
        {
            Entities = new ConcurrentDictionary<int, Entity>();
            Interactives = new ConcurrentDictionary<int, InteractivObject>();
            Pathmaker = new Pathmaker();
        }

        public void FireEntitiesUpdate()
        {
            EntitiesUpdate?.Invoke();
        }

        public void FireEntityMove(int id, string path)
        {
            EntityMoved?.Invoke(id, path);
        }

        public static void Initialize()
        {
            Maps = new Dictionary<int, Map>();
            var dll = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Maps.json"))
            {
                string result = reader.ReadToEnd();
                Maps = JsonConvert.DeserializeObject<List<Map>>(result).ToDictionary(c => c.MapId, c => DecompressMap(c));
            }
        }

        public static Map DecompressMap(Map map)
        {
            map.Cells = new Cell[map.MapData.Length / 10];
            //map.Interactives = new Dictionary<int, InteractivObject>();
            string cellsValues;

            for (int i = 0; i < map.MapData.Length; i += 10)
            {
                cellsValues = map.MapData.Substring(i, 10);
                map.Cells[i / 10] = DecompressCell(map, cellsValues, Convert.ToInt16(i / 10));
            }

            return map;
        }

        public static Cell DecompressCell(Map map, string cellData, short cellId)
        {
            byte[] cellInformations = new byte[cellData.Length];

            for (int i = 0; i < cellData.Length; i++)
                cellInformations[i] = Convert.ToByte(Hash.get_Hash(cellData[i]));

            int mapWidth = map.Width;
            int loc5 = cellId / ((mapWidth * 2) - 1);
            int loc6 = cellId - (loc5 * ((mapWidth * 2) - 1));
            int loc7 = loc6 % mapWidth;
            short interactiv = ((cellInformations[7] & 2) >> 1) != 0 ? Convert.ToInt16(((cellInformations[0] & 2) << 12) + ((cellInformations[7] & 1) << 12) + (cellInformations[8] << 6) + cellInformations[9]) : Convert.ToInt16(-1);

            Cell cell = new Cell()
            {
                CellId = cellId,
                Type = (CellTypes)((cellInformations[2] & 56) >> 3),
                IsActive = (cellInformations[0] & 32) >> 5 != 0,
                IsInLineOfSight = (cellInformations[0] & 1) != 1,
                InteractiveObject = interactiv,
                LayerObject2 = Convert.ToInt16(((cellInformations[0] & 2) << 12) + ((cellInformations[7] & 1) << 12) + (cellInformations[8] << 6) + cellInformations[9]),
                LayerObject1 = Convert.ToInt16(((cellInformations[0] & 4) << 11) + ((cellInformations[4] & 1) << 12) + (cellInformations[5] << 6) + cellInformations[6]),
                LayerGroundLevel = Convert.ToByte(cellInformations[1] & 15),
                LayerGroundSlope = Convert.ToByte((cellInformations[4] & 60) >> 2),
                X = (cellId - ((mapWidth - 1) * (loc5 - loc7))) / mapWidth,
                Y = loc5 - loc7
            };
            if (InteractivObject.InteractivesObjects.ContainsKey(interactiv))
                map.Interactives.TryAdd(cellId, InteractivObject.InteractivesObjects[interactiv].Clone());
            return cell;
        }
        public Cell GetCellFromId(short prmCellId) => Cells[prmCellId];

        public int GetCellHeight(int nCellNum)
        {
            var _loc3 = Cells[nCellNum];
            var _loc4 = _loc3.LayerGroundSlope == 1 ? 0 : 5.000000E-001;
            int? _loc5 = _loc3.LayerGroundLevel == null ? 0 : _loc3.LayerGroundSlope - 7;
            return (int)(_loc5 + _loc4);
        }

        public Cell GetCaseCoordonnee(int nNum)
        {
            var _loc4 = this.Width;
            var _loc5 = Math.Floor((double)(nNum / (_loc4 * 2 - 1)));
            var _loc6 = nNum - _loc5 * (_loc4 * 2 - 1);
            var _loc7 = _loc6 % _loc4;
            var _loc8 = new Cell();
            _loc8.Y = (int)(_loc5 - _loc7);
            _loc8.X = (nNum - (_loc4 - 1) * _loc8.Y) / _loc4;
            return (_loc8);
        }

        public bool CheckView(int startCell, int endCell)
        {
            var _loc5 = GetCaseCoordonnee(startCell);
            var _loc6 = GetCaseCoordonnee(endCell);
            var _loc7 = Cells[startCell];
            var _loc8 = Cells[endCell];
            _loc5.Z = GetCellHeight(startCell);
            _loc6.Z = GetCellHeight(endCell);
            var _loc11 = _loc6.Z - _loc5.Z;
            var _loc12 = Math.Max(Math.Abs(_loc5.Y - _loc6.Y), Math.Abs(_loc5.X - _loc6.X));
            var _loc13 = (_loc5.Y - _loc6.Y) / (_loc5.X - _loc6.X);
            var _loc14 = _loc5.Y - _loc13 * _loc5.X;
            var _loc15 = _loc6.X - _loc5.X >= 0 ? 1 : -1;
            var _loc16 = _loc6.Y - _loc5.Y >= 0 ? 1 : -1;
            var _loc17 = _loc5.Y;
            var _loc18 = _loc5.X;
            var _loc19 = _loc6.X * _loc15;
            var _loc20 = _loc6.Y * _loc16;
            var _loc27 = _loc5.X + 5.000000E-001 * _loc15;
            int _loc26;

            while ((_loc27 * _loc15) <= _loc19)
            {
                var _loc25 = _loc13 * _loc27 + _loc14;
                int licking = _loc6.Y - _loc5.Y >= 0 ? 1 : -1;
                double _loc21 = 0;
                double _loc22 = 0;
                if (licking > 0)
                {
                    _loc21 = Math.Round(_loc25);
                    _loc22 = Math.Ceiling(_loc25 - 5.000000E-001);
                }
                else
                {
                    _loc21 = Math.Ceiling(_loc25 - 5.000000E-001);
                    _loc22 = Math.Round(_loc25);
                } // end else if
                _loc26 = _loc17;

                while (_loc26 * _loc16 <= _loc22 * _loc16)
                {
                    if (!checkCellView((int)(_loc27 - _loc15 / 2), _loc26, false, _loc5, _loc6, _loc11, _loc12))
                    {
                        return false;
                    } // end if
                } // end while
                _loc17 = (int)_loc21;
            } // end while
            _loc26 = _loc17;

            while (_loc26 * _loc16 <= _loc6.Y * _loc16)
            {
                if (!checkCellView((int)(_loc27 - 5.000000E-001 * _loc15), _loc26, false, _loc5, _loc6, _loc11, _loc12))
                {
                    return (false);
                } // end if
            } // end while
            if (!checkCellView((int)(_loc27 - 5.000000E-001 * _loc15), _loc26 - _loc16, true, _loc5, _loc6, _loc11, _loc12))
            {
                return (false);
            } // end if
            return (true);
        }

        public bool checkCellView(int _loc3_, int _loc4_, bool _loc5_, Cell _loc6_, Cell _loc7_, int _loc8_, int _loc9_)
        {
            var _loc10_ = GetCaseNum(_loc3_, _loc4_);
            var _loc11_ = Cells[_loc10_];
            var _loc12_ = Math.Max(Math.Abs(_loc6_.Y - _loc4_), Math.Abs(_loc6_.X - _loc3_));
            var _loc13_ = _loc12_ / _loc9_ * _loc8_ + _loc6_.Z;
            var _loc14_ = GetCellHeight(_loc10_);
            var _loc15_ = !((_loc12_ == 0 || (_loc5_ || _loc7_.X == _loc3_ && _loc7_.Y == _loc4_))) ? true : false;
            if (!_loc11_.IsInLineOfSight && (_loc11_.IsActive && (_loc14_ <= _loc13_ && !_loc15_)))
            {
                return true;
            }
            if (_loc5_)
            {
                return true;
            }
            return false;
        }

        public int GetCaseNum(int x, int y)
        {
            var _loc5 = Width;
            return (x * _loc5 + y * (_loc5 - 1));
        }
    }
}
