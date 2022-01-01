using System;
using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Util;

namespace Demonomania.Scripts.MazeGen.Algo {
    public abstract class AbstractMazeGen {
        private readonly Grid _grid;

        public int Width => _grid.Width;
        public int Height => _grid.Height;

        protected Grid Grid => _grid;

        protected AbstractMazeGen(Grid grid) {
            _grid = grid;
        }

        public abstract void Generate(bool exit);

        protected void AddExit(Directions directions) => _grid.AddExit(directions);

        protected void FillGrid() => _grid.FillGrid();

        protected void Connect(Cell fst, Cell snd) {
            _grid.Connect(fst, snd);
        }

        protected Cell GetRandomCell(int seed) => _grid.GetRandomCell(seed);

        protected bool[,] GetVisitedBooleanMask() => _grid.GetVisited();

        protected int GetVisitedBooleanMaskCount() => _grid.GetVisitedCount();

        public Cell this[int i, int j] {
            get => _grid[i, j];
            protected set => _grid[i, j] = value;
        }

        protected List<Cell> GetNeighbors(Cell cell) => _grid.GetNeighbors(cell);
    }
}
