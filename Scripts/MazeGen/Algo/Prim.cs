using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class Prim : RandomMaze {
        private readonly bool[,] _visited;

        public Prim(int width, int height, int? seed = null) : base(width, height, seed) {
            _visited = new bool[Width, Height];
        }

        public override void Generate(bool exit) {
            FillGrid();

            var first = base[Random.Next(Width), Random.Next(Height)];
            _visited[first.X, first.Y] = true;

            var frontier = GetNeighbors(first).ToHashSet();

            while (frontier.Count > 0) {
                var toAdd = frontier.ElementAt(Random.Next(frontier.Count));
                frontier.Remove(toAdd);
                var mazeCandidates = GetNeighbors(toAdd).Where(c => _visited[c.X, c.Y]).ToList();
                var fromMaze       = mazeCandidates[Random.Next(mazeCandidates.Count)];
                _visited[toAdd.X, toAdd.Y] = true;
                Connect(toAdd, fromMaze);
                frontier.UnionWith(GetFrontier(toAdd));
            }

            if (exit) {
                AddExit();
            }
        }

        private IEnumerable<Cell> GetFrontier(Cell cell) => GetNeighbors(cell).Where(c => !_visited[c.X, c.Y]).ToList();

        private IEnumerable<Cell> GetNeighbors(Cell cell) {
            var list = new List<Cell>(4);
            if (cell.X > 0) {
                list.Add(base[cell.X - 1, cell.Y]);
            }

            if (cell.X < Width - 1) {
                list.Add(base[cell.X + 1, cell.Y]);
            }

            if (cell.Y > 0) {
                list.Add(base[cell.X, cell.Y - 1]);
            }

            if (cell.Y < Height - 1) {
                list.Add(base[cell.X, cell.Y + 1]);
            }

            return list;
        }
    }
}
