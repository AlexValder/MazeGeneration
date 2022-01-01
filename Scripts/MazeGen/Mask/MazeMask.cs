using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Algo;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Mask {
    public class MazeMask {
        public HashSet<(int, int)> Disabled { get; } = new HashSet<(int, int)>();

        public Cell[,] Process(Cell[,] cells) {
            var width  = cells.Length;
            var height = cells.GetLength(1);

            // undo any previous changes
            for (var i = 0; i < width; ++i) {
                for (var j = 0; j < height; ++j) {
                    cells[i, j].Enabled = true;
                }
            }

            foreach (var (i, j) in Disabled) {
                if (i >= 0 && i < width && j >= 0 && j < height) {
                    cells[i, j].Enabled = false;
                }
            }

            return cells;
        }
    }
}
