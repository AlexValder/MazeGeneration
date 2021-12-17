using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class BinaryTree : RandomMaze {
        public BinaryTree(int width, int height, int? seed = null) : base(width, height, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    var neighbors = new List<Cell>(2);
                    if (i < Width - 1) {
                        neighbors.Add(base[i + 1, j]);
                    }

                    if (j < Height - 1) {
                        neighbors.Add(base[i, j + 1]);
                    }

                    if (neighbors.Count > 0) {
                        Connect(base[i, j], neighbors[Random.Next(neighbors.Count)]);
                    }
                }
            }

            if (exit) {
                AddExit();
            }
        }
    }
}
