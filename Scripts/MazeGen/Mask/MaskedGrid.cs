using System;
using System.Collections.Generic;
using System.Xml;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Mask {
    public class MaskedGrid : Grid {
        public MazeMask Mask { get; }

        public MaskedGrid(int width, int height, MazeMask mask) : base(width, height) {
            Mask = mask;
        }

        public override void FillGrid() {
            base.FillGrid();
            foreach (var (i, j) in Mask.Disabled) {
                this[i, j].Enabled = false;
            }
        }

        public override List<Cell> GetNeighbors(Cell cell) {
            var list = new List<Cell>(4);

            if (cell.X > 0 && this[cell.X - 1, cell.Y].Enabled) {
                list.Add(this[cell.X - 1, cell.Y]);
            }

            if (cell.Y > 0 && this[cell.X, cell.Y - 1].Enabled) {
                list.Add(this[cell.X, cell.Y - 1]);
            }

            if (cell.X < Width - 1 && this[cell.X + 1, cell.Y].Enabled) {
                list.Add(this[cell.X + 1, cell.Y]);
            }

            if (cell.Y < Height - 1 && this[cell.X, cell.Y + 1].Enabled) {
                list.Add(this[cell.X, cell.Y + 1]);
            }

            return list;
        }

        public override bool[,] GetVisited() {
            var visited = base.GetVisited();
            foreach (var (i, j) in Mask.Disabled) {
                visited[i, j] = true;
            }
            return visited;
        }

        public override int GetVisitedCount() => Width * Height - Mask.Disabled.Count;

        public override Cell GetRandomCell(int? seed = null) {
            if (Mask.Disabled.Count == 0) {
                return base.GetRandomCell(seed);
            }

            if (Mask.Disabled.Count >= Width * Height) {
                return null;
            }

            return InnerRandomCell();
        }

        public override Cell GetRandomNorthernCell(Random rand) {
            for (var i = 0; i < Width; ++i) {
                var cell = this[rand.Next(Width), 0];
                if (cell.Enabled) return cell;
            }

            throw new ArgumentOutOfRangeException();
        }

        public override Cell GetRandomSouthernCell(Random rand) {
            for (var i = 0; i < Width; ++i) {
                var cell = this[rand.Next(Width), Height - 1];
                if (cell.Enabled) return cell;
            }

            throw new ArgumentOutOfRangeException();
        }

        public override Cell GetRandomEasternCell(Random rand) {
            for (var j = 0; j < Height; ++j) {
                var cell = this[0, rand.Next(Height)];
                if (cell.Enabled) return cell;
            }

            throw new ArgumentOutOfRangeException();
        }

        public override Cell GetRandomWesternCell(Random rand) {
            for (var j = 0; j < Height; ++j) {
                var cell = this[Width - 1, rand.Next(Height)];
                if (cell.Enabled) return cell;
            }

            throw new ArgumentOutOfRangeException();
        }

        private Cell InnerRandomCell() {
            while (true) {
                var cell = base.GetRandomCell();
                if (cell.Enabled) return cell;
            }
        }
    }
}
