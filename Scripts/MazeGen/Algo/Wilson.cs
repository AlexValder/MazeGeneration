using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    [Maskable]
    public class Wilson : RandomMaze {
        private List<Cell> _unvisited;

        public Wilson(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            _unvisited = new List<Cell>(Width * Height);
            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    if (base[i, j].Enabled) {
                        _unvisited.Add(base[i, j]);
                    }
                }
            }

            var first = _unvisited[Random.Next(_unvisited.Count)];
            _unvisited.Remove(first);

            while (_unvisited.Count > 0) {
                var cell = _unvisited[Random.Next(_unvisited.Count)];
                var path = new List<Cell> {
                    cell
                };

                while (_unvisited.Contains(cell)) {
                    var neighbors = GetNeighbors(cell);
                    cell = neighbors[Random.Next(neighbors.Count)];
                    var index = path.IndexOf(cell);

                    if (index != -1) {
                        path = path.GetRange(0, index + 1);
                    } else {
                        path.Add(cell);
                    }
                }

                for (var q = 0; q < path.Count - 1; ++q) {
                    Connect(base[path[q].X, path[q].Y], base[path[q + 1].X, path[q + 1].Y]);
                    _unvisited.Remove(path[q]);
                }
            }

            if (exit) {
                AddExit();
            }
        }
    }
}
