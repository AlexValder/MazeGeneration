using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    [Maskable]
    public class RecursiveBacktracker : RandomMaze {
        public RecursiveBacktracker(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            StartAt(GetRandomCell(Random.Next()));

            if (exit) {
                AddExit();
            }
        }

        private void StartAt(Cell cell) {
            var stack = new List<Cell> {cell};

            while (stack.Count > 0) {
                var current   = stack[stack.Count - 1];
                var neighbors = GetNeighbors(current).Where(c => c.Directions == Directions.None).ToList();

                if (neighbors.Count == 0) {
                    stack.Remove(current);
                } else {
                    var neighbor = neighbors[Random.Next(neighbors.Count)];
                    Connect(base[current.X, current.Y], base[neighbor.X, neighbor.Y]);
                    stack.Add(neighbor);
                }
            }
        }
    }
}
