using System;
using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class AldousBroder : RandomMaze {
        private bool[,] _visited;

        public AldousBroder(int width, int height, int? seed = null) : base(width, height, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            _visited = new bool[Width, Height];

            var cell = base[Random.Next(Width), Random.Next(Height)];
            _visited[cell.X, cell.Y] = true;
            var unvisited = Width * Height - 1;

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
