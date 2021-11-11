using System;

namespace Demonomania.Scripts.MazeGen {
    public sealed class Kruskal : RandomMaze {
        public Kruskal(int width, int height, int? seed = null) : base(width, height, seed) { }

        public override void Generate() {
            base.Generate();

            for (var i = 0; i < Width; ++i) {
                this[i, 0].Directions          &= Directions.Left;
                this[i, Height - 1].Directions &= Directions.Right;
            }

            for (var j = 0; j < Height; ++j) {
                this[0, j].Directions         &= Directions.Up;
                this[Width - 1, j].Directions &= Directions.Down;
            }
        }
    }
}
