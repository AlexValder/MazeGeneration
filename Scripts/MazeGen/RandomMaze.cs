﻿using System;

namespace Demonomania.Scripts.MazeGen {
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

        public override void Generate() {
            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    base[i, j] = new Cell(_cells[Random.Next(_cells.Length)]) {
                        X = i,
                        Y = j,
                    };
                }
            }
        }
    }
}
