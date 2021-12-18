using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class RecursiveBacktracker : RandomMaze {
        public RecursiveBacktracker(int width, int height, int? seed = null) : base(width, height, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            StartAt(base[Random.Next(Width), Random.Next(Height)]);

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
