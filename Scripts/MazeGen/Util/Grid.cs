using System;
using System.Collections.Generic;
using Serilog;

namespace Demonomania.Scripts.MazeGen.Util {
    public class Grid {
        private readonly Cell[,] _innerGrid;

        public int Width { get; }
        public int Height { get; }

        public Grid(int width, int height) {
            _innerGrid = new Cell[width, height];
            Width      = width;
            Height     = height;
        }

        public Cell this[int i, int j] {
            get => _innerGrid[i, j];
            set => _innerGrid[i, j] = value;
        }

        public virtual void FillGrid() {
            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    this[i, j] = new Cell(Directions.None) {
                        X = i,
                        Y = j,
                    };
                }
            }
        }

        public virtual List<Cell> GetNeighbors(Cell cell) {
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

        public virtual Cell GetRandomCell(int? seed = null) {
            var rand = seed.HasValue ? new Random(seed.Value) : new Random();
            return _innerGrid[rand.Next(Width), rand.Next(Height)];
        }

        public void Connect(Cell fst, Cell snd) {
            if (!fst.Enabled || !snd.Enabled) {
                Log.Logger.Debug(
                    "Tried to connect [{X1}, {Y1}] ({Status1}) and [{X2}, {Y2}] ({Status2})",
                    fst.X, fst.Y, fst.Enabled ? "Enabled" : "Disabled",
                    snd.X, snd.Y, snd.Enabled ? "Enabled" : "Disabled"
                );
                return;
            }

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

        public virtual Cell GetRandomNorthernCell(Random rand) {
            return this[rand.Next(Width), 0];
        }

        public virtual Cell GetRandomSouthernCell(Random rand) {
            return this[rand.Next(Width), Height - 1];
        }

        public virtual Cell GetRandomEasternCell(Random rand) {
            return this[0, rand.Next(Height)];
        }

        public virtual Cell GetRandomWesternCell(Random rand) {
            return this[Width - 1, rand.Next(Height)];
        }

        public void AddExit(Directions side, int? seed = null) {
            var rand = seed.HasValue ? new Random(seed.Value) : new Random();
            switch (side) {
                case Directions.Up:
                    GetRandomNorthernCell(rand).Directions |= Directions.Up;
                    break;
                case Directions.Right:
                    GetRandomWesternCell(rand).Directions |= Directions.Right;
                    break;
                case Directions.Down:
                    GetRandomSouthernCell(rand).Directions |= Directions.Down;
                    break;
                case Directions.Left:
                    GetRandomEasternCell(rand).Directions |= Directions.Left;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(side.ToString());
            }
        }

        public virtual bool[,] GetVisited() => new bool[Width, Height];

        public virtual int GetVisitedCount() => Width * Height;
    }
}
