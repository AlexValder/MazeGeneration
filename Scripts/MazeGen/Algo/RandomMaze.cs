using System;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class RandomMaze : AbstractMazeGen {
        protected Random Random { get; }

        private readonly Directions[] _cells = {
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

        public RandomMaze(int width, int height, int? seed = null) : base(width, height) {
            Random = seed == null ? new Random() : new Random(seed.Value);
        }

        public override void Generate(bool exit) {
            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    base[i, j] = new Cell(_cells[Random.Next(_cells.Length)]) {
                        X = i,
                        Y = j,
                    };
                }
            }
        }

        protected override void AddExit() {
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

        private void AddExit(Directions side) {
            switch (side) {
                case Directions.Up:
                    base[Random.Next(Width), 0].Directions |= Directions.Up;
                    break;
                case Directions.Right:
                    base[Width - 1, Random.Next(Height)].Directions |= Directions.Right;
                    break;
                case Directions.Down:
                    base[Random.Next(Width), Height - 1].Directions |= Directions.Down;
                    break;
                case Directions.Left:
                    base[0, Random.Next(Height)].Directions |= Directions.Left;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(side.ToString());
            }
        }
    }
}
