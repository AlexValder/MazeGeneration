using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public sealed class Kruskal : RandomMaze {
        private class CellId {
            public Cell Cell;
            public int Id;
        }
        public Kruskal(int width, int height, int? seed = null) : base(width, height, seed) { }

        public override void Generate() {
            var grid  = new List<List<CellId>>(Width * Height);
            var edges = new HashSet<(CellId, CellId)>();

            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    grid.Add(new List<CellId>());
                    base[i, j] = new Cell(Directions.None) {X = i, Y = j};
                    grid[i * Width + j].Add(new CellId {
                        Cell = base[i, j],
                        Id   = i * Width + j,
                    });
                }
            }

            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    if (i + 1 < Width) {
                        edges.Add((grid[i * Width + j][0], grid[(i + 1) * Width + j][0]));
                    }

                    if (j + 1 < Height) {
                        edges.Add((grid[i * Width + j][0], grid[i * Width + j + 1][0]));
                    }
                }
            }

            while (edges.Count > 0) {
                var edge = edges.ElementAt(Random.Next(edges.Count));
                var (fst, snd) = edge;
                if (fst.Id != snd.Id) {
                    Connect(fst.Cell, snd.Cell);
                    MergeBucket(grid, fst, snd);
                }

                edges.Remove(edge);
            }
        }

        private static void MergeBucket(IReadOnlyList<List<CellId>> buckets, CellId @from, CellId to) {
            if (@from.Id == to.Id) {
                return;
            }

            var first  = buckets[from.Id];
            var second = buckets[to.Id];

            foreach (var cellId in first) {
                cellId.Id = to.Id;
            }

            second.AddRange(first);
            first.Clear();
        }
    }
}
