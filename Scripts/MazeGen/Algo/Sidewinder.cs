﻿using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class Sidewinder : RandomMaze {
        public Sidewinder(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            for (var i = 0; i < Width - 1; ++i) {
                Connect(base[i, 0], base[i + 1, 0]);
            }

            for (var j = 1; j < Height; ++j) {
                var run = new List<Cell>(Width);

                for (var i = 0; i < Width; ++i) {
                    if (!base[i, j].Enabled) {
                        continue;
                    }

                    run.Add(base[i, j]);
                    if (i + 1 < Width && base[i + 1, j].Enabled && Random.Next() % 2 == 0) {
                        Connect(base[i, j], base[i + 1, j]);
                    } else {
                        var cell = run[Random.Next(run.Count)];
                        Connect(base[cell.X, cell.Y], base[cell.X, cell.Y - 1]);
                        run.Clear();
                    }
                }
            }

            if (exit) {
                AddExit();
            }
        }
    }
}
