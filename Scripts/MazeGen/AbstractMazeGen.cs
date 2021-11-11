namespace Demonomania.Scripts.MazeGen {
    public abstract class AbstractMazeGen {
        private readonly Cell[,] _innerGrid;
        public int Width => _innerGrid.GetLength(0);
        public int Height => _innerGrid.GetLength(1);

        protected AbstractMazeGen(int width, int height) {
            _innerGrid = new Cell[width, height];
        }

        public abstract void Generate();

        public Cell this[int i, int j] {
            get => _innerGrid[i, j];
            protected set => _innerGrid[i, j] = value;
        }
    }
}
