using System;
using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public abstract class AbstractMazeGen {
        private readonly Cell[,] _innerGrid;
        public int Width => _innerGrid.GetLength(0);
        public int Height => _innerGrid.GetLength(1);

        protected AbstractMazeGen(int width, int height) {
            _innerGrid = new Cell[width, height];
        }

        public abstract void Generate(bool exit);

        protected abstract void AddExit();

        protected void FillGrid() {
            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    this[i, j] = new Cell(Directions.None) {
                        X = i,
                        Y = j,
                    };
                }
            }
        }

        protected void Connect(Cell fst, Cell snd) {
            if (fst.X - snd.X == -1) {
                // left <-> right
                this[fst.X, fst.Y].Directions |= Directions.Right;
                this[snd.X, snd.Y].Directions |= Directions.Left;
            } else if (fst.X - snd.X == 1) {
                // left <-> right
                this[fst.X, fst.Y].Directions |= Directions.Left;
                this[snd.X, snd.Y].Directions |= Directions.Right;
            } else if (fst.Y - snd.Y == -1) {
                // up <-> down
                this[fst.X, fst.Y].Directions |= Directions.Down;
                this[snd.X, snd.Y].Directions |= Directions.Up;
            } else if (fst.Y - snd.Y == 1) {
                this[fst.X, fst.Y].Directions |= Directions.Up;
                this[snd.X, snd.Y].Directions |= Directions.Down;
            } else {
                throw new ArgumentException("Cells should be next to each other");
            }
        }

        public Cell this[int i, int j] {
            get => _innerGrid[i, j];
            protected set => _innerGrid[i, j] = value;
        }

        protected List<Cell> GetNeighbors(Cell cell) {
            var list = new List<Cell>(4);

            if (cell.X > 0) {
                list.Add(this[cell.X - 1, cell.Y]);
            }

            if (cell.Y > 0) {
                list.Add(this[cell.X, cell.Y - 1]);
            }

            if (cell.X < Width - 1) {
                list.Add(this[cell.X + 1, cell.Y]);
            }

            if (cell.Y < Height - 1) {
                list.Add(this[cell.X, cell.Y + 1]);
            }

            return list;
        }
    }
}
