using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;
using Godot;

namespace Demonomania.Scripts.MazeGen.Algo {
    [Maskable]
    public sealed class Kruskal : RandomMaze {
        private class CellId {
            public Cell Cell;
            public int Id;
        }
        public Kruskal(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            var grid = new List<List<CellId>>(Width * Height);
            for (var i = 0; i < grid.Capacity; ++i) {
                grid.Add(new List<CellId>());
            }

            var edges  = new HashSet<(CellId, CellId)>();

            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    if (this[i, j].Enabled) {
                        grid[i + j * Width].Add(new CellId {
                            Cell = base[i, j],
                            Id   = i + Width * j,
                        });
                    }
                }
            }

            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    if (!this[i, j].Enabled) {
                        continue;
                    }

                    if (i + 1 < Width) {
                        edges.Add((grid[i + Width * j][0], grid[i + 1 + Width * j][0]));
                    }

                    if (j + 1 < Height) {
                        edges.Add((grid[i + Width * j][0], grid[i + Width * (j + 1)][0]));
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

            if (exit) {
                AddExit();
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
