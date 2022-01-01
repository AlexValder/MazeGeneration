using System.Collections.Generic;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;
using Serilog;

namespace Demonomania.Scripts.MazeGen.Algo {
    public class HuntAndKill : RandomMaze {
        private bool[,] _visited;
        private bool _walkedOnce;

        public HuntAndKill(Grid grid, int? seed = null) : base(grid, seed) { }

        public override void Generate(bool exit) {
            FillGrid();

            _visited    = new bool[Width, Height];
            _walkedOnce = false;

            Cell fst;
            while ((fst = Hunt()) != null) {
                Log.Logger.Debug("[Hunt] CELL: ({X};{Y})", fst.X, fst.Y);
                Walk(fst);
            }

            if (exit) {
                AddExit();
            }
        }

        private void Walk(Cell fst) {
            _visited[fst.X, fst.Y] = true;
            while (true) {
                var snd = GetNext(fst);
                if (snd == null) {
                    break;
                }
                Log.Logger.Debug("[Walk] CELL: ({X};{Y})", snd.X, snd.Y);
                _visited[snd.X, snd.Y] = true;
                Connect(fst, snd);
                fst = snd;
            }
            _walkedOnce = true;
        }

        private Cell Hunt() {
            if (!_walkedOnce) {
                return GetRandomCell(Random.Next());
            }

            for (var i = 0; i < Width; ++i) {
                for (var j = 0; j < Height; ++j) {
                    if (_visited[i, j]) {
                        continue;
                    }

                    if (i < Width - 1 && _visited[i + 1, j]) {
                        Connect(base[i, j], base[i + 1, j]);
                        return base[i, j];
                    }
                    if (i > 0 && _visited[i - 1, j]) {
                        Connect(base[i, j], base[i - 1, j]);
                        return base[i, j];
                    }

                    if (j < Height - 1 && _visited[i, j + 1]) {
                        Connect(base[i, j], base[i, j + 1]);
                        return base[i, j];
                    }

                    if (j > 0 && _visited[i, j - 1]) {
                        Connect(base[i, j], base[i, j - 1]);
                        return base[i, j];
                    }
                }
            }

            return null;
        }

        private Cell GetNext(Cell cell) {
            var cells = GetUnvisitedNeighbors(cell);
            if (cells.Count == 0) {
                return null;
            }

            var selected = cells[Random.Next(cells.Count)];
            return selected;
        }

        private IList<Cell> GetUnvisitedNeighbors(Cell cell) {
            var list = new List<Cell>(4);
            if (cell.X > 0 && !_visited[cell.X - 1, cell.Y]) {
                list.Add(base[cell.X - 1, cell.Y]);
            }

            if (cell.X < Width - 1 && !_visited[cell.X + 1, cell.Y]) {
                list.Add(base[cell.X + 1, cell.Y]);
            }

            if (cell.Y > 0 && !_visited[cell.X, cell.Y - 1]) {
                list.Add(base[cell.X, cell.Y - 1]);
            }

            if (cell.Y < Height - 1 && !_visited[cell.X, cell.Y + 1]) {
                list.Add(base[cell.X, cell.Y + 1]);
            }

            return list;
        }
    }
}
