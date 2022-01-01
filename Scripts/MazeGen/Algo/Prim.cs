using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class Prim : RandomMaze {
        private bool[,] _visited;

        public Prim(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            _visited = new bool[Width, Height];

            var first = GetRandomCell(Random.Next());
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
    }
}
