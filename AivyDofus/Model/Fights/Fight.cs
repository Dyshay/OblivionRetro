using AivyDofus.Model.Fights;
using AivyDofus.Model.Maps;
using AivyDofus.Protocol;
using DeepBot.Model.Account.Game.Maps;
using DeepBot.Model.Account.Game.Maps.Cells;
using DeepBot.Model.Account.Game.Maps.Entities;
using DeepBot.Utilities.Pathfinding;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyData.Model.Fights
{
    public class Fight
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public Account Account { get; private set; }
        public ConcurrentDictionary<int, AbstractFighter> Fighters;
        private ConcurrentDictionary<int, AbstractFighter> Defenders;
        private ConcurrentDictionary<int, AbstractFighter> Attackers;
        public List<Cell> StartingCells;
        public AbstractFighter Current;
        public AbstractFighter Focus;
        public Map Map;
        public Func<Task> OnTurnStart;

        public IEnumerable<AbstractFighter> DefendersAlive => Defenders.Values.Where(def => def.IsAlive);
        public IEnumerable<AbstractFighter> AttackersAlive => Attackers.Values.Where(def => def.IsAlive);
        public int DefendersAliveCount => DefendersAlive.Count();
        public int AttackersAliveCount => AttackersAlive.Count();

        public Fight(Account account)
        {
            Account = account;
            Fighters = new ConcurrentDictionary<int, AbstractFighter>();
            Defenders = new ConcurrentDictionary<int, AbstractFighter>();
            Attackers = new ConcurrentDictionary<int, AbstractFighter>();
            StartingCells = new List<Cell>();
            OnTurnStart += onAttributeDistance;
            OnTurnStart += onTurnStart;
        }

        private async Task onAttributeDistance()
        {
            foreach (var defender in Defenders)
            {
                (defender.Value as MonsterFighter).Distance = (short)defender.Value.Cell.GetDistanceBetweenCells(Current.Cell);
            }
        }

        public async Task FighStart()
        {
            Map = Account.Character.Map;
            Account.State = Enums.AccountState.FIGHTING;
            //Account.Character.Spells = Account.Character.Spells.Where(spell => spell.Key == 161);
        }

        public async Task onTurnStart()
        {
            onAttributeDistance();
            Current.Pm = 3;
            Focus = GetNearestEnemy();
            if (CanTouch(Focus))
            {
                await Task.Delay(500);
                await ProcessSpell();
                await Task.Delay(500);
                ProcessMove();
                await onTurnFinish();
            }
            else
            {
                await Task.Delay(500);
                ProcessMove();
                if (CanTouch(Focus))
                {
                    await Task.Delay(500);
                    await ProcessSpell();
                }
                await onTurnFinish();
            }
        }

        private async Task onTurnFinish()
        {
            await Task.Delay(500);
            Account.Send("Gt");
        }

        public void onFightEnd()
        {
            Account.State = Enums.AccountState.IDLE;
            Fighters.Clear();
            Attackers.Clear();
            Defenders.Clear();
            StartingCells.Clear();
        }

        public short GetCellNearOrFar(bool isNear, List<Cell> available_cells)
        {
            short cell_id = -1;
            int distance_total = -1;

            foreach (Cell cell in available_cells)
            {
                int tempory_distance = GetDistanceFromEnemy(cell);

                if (cell_id == -1 || (isNear && tempory_distance < distance_total) || (!isNear && tempory_distance > distance_total))
                {
                    cell_id = cell.CellId;
                    distance_total = tempory_distance;
                }
            }

            return cell_id;
        }

        public AbstractFighter GetFighterById(int id)
        {
            if (Current != null && Current.Id == id)
                return Current;
            else if (Fighters.TryGetValue(id, out AbstractFighter fighter))
                return fighter;

            return null;
        }

        public int GetDistanceFromEnemy(Cell actual_cell) => DefendersAlive.Sum(def => actual_cell.GetDistanceBetweenCells(def.Cell));

        public void AddFighter(AbstractFighter fighter)
        {
            Fighters.TryAdd(fighter.Id, fighter);
            if (fighter.Teams == 0)
                Attackers.TryAdd(fighter.Id, fighter);
            else
                Defenders.TryAdd(fighter.Id, fighter);

            if (fighter.Id == Account.Character.Id)
                Current = fighter;
        }

        public void UpdateCellFighter(AbstractFighter fighter, short cell_id)
        {
            Fighters[fighter.Id].Cell = Account.Character.Map.Cells[cell_id];

            if (Current.Id == fighter.Id)
                Current.Cell = Account.Character.Map.Cells[cell_id];

            if (fighter.Teams == 0)
                Attackers[fighter.Id].Cell = Account.Character.Map.Cells[cell_id];
            else
                Defenders[fighter.Id].Cell = Account.Character.Map.Cells[cell_id];
        }

        public void UpdateFighter(AbstractFighter fighter, int health, bool isAlive, byte pa, byte pm, int healt_max, int cell)
        {
            if (Current.Id == fighter.Id)
            {
                Current.Health = health;
                Current.IsAlive = isAlive;
                Current.Pa = pa;
                Current.Pm = pm;
                Current.HealthMax = healt_max;
                Current.Cell = Account.Character.Map.Cells[cell];
            }

            if (fighter.Teams == 0)
            {
                Attackers[fighter.Id].Health = health;
                Attackers[fighter.Id].IsAlive = isAlive;
                Attackers[fighter.Id].Pa = pa;
                Attackers[fighter.Id].Pm = pm;
                Attackers[fighter.Id].HealthMax = healt_max;
                Attackers[fighter.Id].Cell = Account.Character.Map.Cells[cell];
            }
            else
            {
                Defenders[fighter.Id].Health = health;
                Defenders[fighter.Id].IsAlive = isAlive;
                Defenders[fighter.Id].Pa = pa;
                Defenders[fighter.Id].Pm = pm;
                Defenders[fighter.Id].HealthMax = healt_max;
                Defenders[fighter.Id].Cell = Account.Character.Map.Cells[cell];
            }
        }

        public bool isFarTo(Cell cell)
        {
            bool isFar = false;
            Defenders.Values.ToList().ForEach(c =>
            {
                if (cell.GetDistanceBetweenCells(c.Cell) > 5)
                {
                    isFar = true;
                }

            });
            return isFar;
        }

        public bool CanTouch(AbstractFighter enemy)
        {
            var fighter = Current as CharacterFighter;

            var spell = fighter.Spells.First(c => c.Key == 161).Value;
            return (Focus as MonsterFighter).Distance <= spell.Stats.First(c => c.Key == spell.Level).Value.RangeMaximum + fighter.Stats.PO.Total && Pathfinding.BresenhamLine(this, Current.Cell.CellId, Focus.Cell.CellId);
        }

        public void ProcessMove()
        {
            var fighter = Current as CharacterFighter;

            var spell = fighter.Spells.First(c => c.Key == 161).Value;

            Map mapa = Account.Character.Map;
            mapa.Pathmaker = new AivyDofus.Model.Maps.Pathmaker(mapa);

            var possibleCells = Current.Cell.GetAdjacents(mapa.Cells, Current.Pm);
            Cell cell = null;
            var accessibleCells = possibleCells.FirstOrDefault(cellule => Pathfinding.BresenhamLine(this, cellule.CellId, Focus.Cell.CellId) && cellule.IsWalkable);
            var cellWidthPM = possibleCells.Where(cellule => Pathfinding.BresenhamLine(this, cellule.CellId, Focus.Cell.CellId) && cellule.GetDistanceBetweenCells(Current.Cell) == Current.Pm && cellule.IsWalkable && isFarTo(cellule));
            if (accessibleCells != null)
            {
                if (cellWidthPM != null)
                {
                    var random = new Random().Next(0, cellWidthPM.Count());
                    cell = cellWidthPM.ToArray()[random];
                }
                else
                {
                    cell = accessibleCells;
                }
            }
            else
            {
                cell = possibleCells.LastOrDefault();
            }
            if (cell != null)
            {
                var nodes = PathFinder.Instance.GetPath(Account.Character.Map, Current.Cell.CellId, cell.CellId, false);
                var stringnodes = PathFinderUtils.Instance.GetPathfindingString(nodes);
                var pathString = mapa.Pathmaker.FindPathAsString(Current.Cell.CellId, cell.CellId, false, Current.Pm, Map.Cells.Where(c => (c.IsWalkable)).Select(c => (int)c.CellId).Concat(Fighters.Values.Where(c => c.IsAlive).Select(c => (int)c.Cell.CellId)));
                Current.Cell = cell;
                Account.Send("GA001" + stringnodes);
            }
        }

        public void ProcessMoveBackup()
        {
            var fighter = Current as CharacterFighter;

            var spell = fighter.Spells.First(c => c.Key == 161).Value;

            Map mapa = Account.Character.Map;
            mapa.Pathmaker = new AivyDofus.Model.Maps.Pathmaker(mapa);

            var possibleCells = Current.Cell.GetAdjacents(mapa.Cells, Account.Character.Fight.Current.Pm);
            Cell cell = null;
            cell = possibleCells.LastOrDefault();
            if (cell != null)
            {
                var nodes = PathFinder.Instance.GetPath(Account.Character.Map, Current.Cell.CellId, cell.CellId, false);
                var stringnodes = PathFinderUtils.Instance.GetPathfindingString(nodes);
                var pathString = mapa.Pathmaker.FindPathAsString(Current.Cell.CellId, cell.CellId, false, Current.Pm, Map.Cells.Where(c => c.IsWalkable).Select(c => (int)c.CellId).Concat(Fighters.Values.Where(c => c.IsAlive).Select(c => (int)c.Cell.CellId)));
                Account.Send("GA001" + stringnodes);
            }
        }

        public async Task ProcessSpell()
        {
            await Task.Delay(600);
            Account.Send("GA300" + 161 + ';' + Focus.Cell.CellId);
        }

        public async Task<bool> CanSpell()
        {
            var fighter = Current as CharacterFighter;

            var spell = fighter.Spells.First(c => c.Key == 161).Value;
            return (Focus as MonsterFighter).Distance <= spell.Stats.First(c => c.Key == spell.Level).Value.RangeMaximum + fighter.Stats.PO.Total;
        }

        public async Task<bool> CanUseSpell()
        {
            Map mapa = Account.Character.Map;
            mapa.Pathmaker = new AivyDofus.Model.Maps.Pathmaker(mapa);
            var spell = (Current as CharacterFighter).Spells.First(c => c.Key == 161).Value;
            var spellStats = spell.Stats.First(c => c.Key == spell.Level).Value;
            if (spellStats.LineOfSight && !Pathfinding.BresenhamLine(this, Current.Cell.CellId, Focus.Cell.CellId))
                return false;
            if (spellStats.LineOnly && !Current.Cell.IsInLine(Focus.Cell, Map.Cells, (Focus as MonsterFighter).Distance))
                return false;
            return true;
        }

        public AbstractFighter GetFighterOnCell(int cellId)
        {
            return DefendersAlive.FirstOrDefault(fighter => fighter.Cell != null && fighter.Cell.CellId == cellId);
        }

        public AbstractFighter GetNearestEnemy()
        {
            return DefendersAlive.Select(fighter => fighter as MonsterFighter).OrderBy(fighter => fighter.Distance).ThenBy(fighter => fighter.Health).First();
        }
    }
}
