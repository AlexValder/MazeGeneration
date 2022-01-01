using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Util;
using Serilog.Core;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class BinaryTree : RandomMaze {
        public BinaryTree(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            for (var i = Width - 1; i >= 0; --i) {
                for (var j = 0; j < Height; ++j) {

                    if (i > 0 && j < Height - 1) {
                        // not at the edge
                        Connect(
                            base[i, j],
                            Random.Next() % 2 == 0 ? base[i - 1, j] : base[i, j + 1]
                        );
                    } else if (i > 0) {
                        // at south edge
                        Connect(base[i, j], base[i - 1, j]);
                    } else if (j < Height - 1) {
                        // at eastern edge
                        Connect(base[i, j], base[i, j + 1]);
                    }
                }
            }

            if (exit) {
                AddExit();
            }
        }
    }
}
