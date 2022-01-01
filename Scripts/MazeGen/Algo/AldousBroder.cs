using System;
using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class AldousBroder : RandomMaze {
        private bool[,] _visited;

        public AldousBroder(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            _visited = GetVisitedBooleanMask();

            var cell = GetRandomCell(Random.Next());
            _visited[cell.X, cell.Y] = true;
            var unvisited = GetVisitedBooleanMaskCount() - 1;

            while (unvisited > 0) {
                var neighbors = GetNeighbors(cell);
                var next = neighbors[Random.Next(neighbors.Count)];

                if (next.Directions == Directions.None) {
                    Connect(base[cell.X, cell.Y], base[next.X, next.Y]);
                    unvisited -= 1;
                }

                cell = next;
            }

            if (exit) {
                AddExit();
            }
        }
    }
}
