using System;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    [Maskable]
    public class RandomMaze : AbstractMazeGen {
        protected Random Random { get; }

        private static readonly Directions[] s_cells = {
            Directions.Up,
            Directions.Right,
            Directions.Left,
            Directions.Down,
            Directions.Up | Directions.Right,
            Directions.Up | Directions.Left,
            Directions.Up | Directions.Down,
            Directions.Right | Directions.Left,
            Directions.Right | Directions.Down,
            Directions.Left | Directions.Down,
        };

        public RandomMaze(Grid grid, int? seed = null) : base(grid) {
            Random = seed == null ? new Random() : new Random(seed.Value);
        }

        public override void Generate(bool exit) {
            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    base[i, j] = new Cell(s_cells[Random.Next(s_cells.Length)]) {
                        X = i,
                        Y = j,
                    };
                }
            }

            if (Grid is MaskedGrid grid) {
                foreach (var (i, j) in grid.Mask.Disabled) {
                    base[i, j].Enabled = false;
                }
            }
        }

        protected void AddExit() {
            var side = (Directions)(1 << Random.Next() % 4);

            switch (side) {
                case Directions.Up:
                case Directions.Down:
                    AddExit(Directions.Up);
                    AddExit(Directions.Down);
                    return;
                case Directions.Right:
                case Directions.Left:
                    AddExit(Directions.Right);
                    AddExit(Directions.Left);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
