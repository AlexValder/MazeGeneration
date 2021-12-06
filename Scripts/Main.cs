using System;
using System.Diagnostics;
using Demonomania.Scripts.MazeGen;
using Demonomania.Scripts.MazeGen.Util;
using Godot;
using Serilog;

namespace Demonomania.Scripts {
    public class Main : Spatial {
        private GridMap _grid;
        private readonly Random _random = new Random();

        private readonly Basis[] _orientations = {
            Basis.Identity,
            Basis.Identity.Rotated(Vector3.Up, Mathf.Pi / 2),
            Basis.Identity.Rotated(Vector3.Up, Mathf.Pi),
            Basis.Identity.Rotated(Vector3.Up, 3 * Mathf.Pi / 2),
        };

        public override void _Ready() {
            SetupLogger();

            _grid = GetNode<GridMap>("GridMap");
            Debug.Assert(_grid != null, "GridMap was null");

            OS.WindowMaximized = Settings.WindowMaximized;
        }

#if DEBUG
        public override void _UnhandledInput(InputEvent @event) {
            if (@event.IsActionPressed("debug_exit")) {
                GetTree().Quit(0);
            }
        }
#endif
        public void CreateRooms(
            int width,
            int height,
            string algorithm,
            string seed,
            Color color,
            bool addExit
        ) {
            ClearRooms();
            var material = new SpatialMaterial {AlbedoColor = color};
            var items    = _grid.MeshLibrary.GetItemList();
            foreach (var index in items) {
                var mesh = _grid.MeshLibrary.GetItemMesh(index);
                mesh.SurfaceSetMaterial(0, material);
            }

            int? seedValue;
            if (string.IsNullOrWhiteSpace(seed)) {
                seedValue = null;
            } else if (int.TryParse(seed, out var seedInt)) {
                seedValue = seedInt;
            } else {
                seedValue = seed.GetHashCode();
            }

            var maze = AlgorithmManager.GetAlgorithm(
                name: algorithm,
                width: width,
                height: height,
                seed: seedValue
            );
            maze.Generate(exit: addExit);

            for (var i = 0; i < width; ++i) {
                for (var j = 0; j < (height < 0 ? width : height); ++j) {
                    var (chosen, orientation) = CellToInt(maze[i, j]);
                    var index = _orientations[orientation].GetOrthogonalIndex();
                    _grid.SetCellItem(
                        x: i,
                        y: 0,
                        z: j,
                        item: chosen,
                        orientation: index
                    );
                }
            }
        }

        private static (int, int) CellToInt(Cell cell) {
            var count = 0;
            if ((cell.Directions & Directions.Up) == Directions.Up) {
                count += 1;
            }

            if ((cell.Directions & Directions.Right) == Directions.Right) {
                count += 1;
            }

            if ((cell.Directions & Directions.Down) == Directions.Down) {
                count += 1;
            }

            if ((cell.Directions & Directions.Left) == Directions.Left) {
                count += 1;
            }

            switch (count) {
                case 0: return (5, 0);
                case 1: {
                    switch (cell.Directions) {
                        case Directions.Up:    return (4, 1);
                        case Directions.Right: return (4, 0);
                        case Directions.Down:  return (4, 3);
                        case Directions.Left:  return (4, 2);
                        default:               throw new ArgumentOutOfRangeException();
                    }
                }
                case 2: {
                    if ((cell.Directions & Directions.Up) == Directions.Up) {
                        if ((cell.Directions & Directions.Down) == Directions.Down) {
                            return (3, 1);
                        }

                        if ((cell.Directions & Directions.Right) == Directions.Right) {
                            return (2, 0);
                        }

                        if ((cell.Directions & Directions.Left) == Directions.Left) {
                            return (2, 1);
                        }
                    }

                    if ((cell.Directions & Directions.Right) == Directions.Right) {
                        if ((cell.Directions & Directions.Left) == Directions.Left) {
                            return (3, 0);
                        }

                        if ((cell.Directions & Directions.Down) == Directions.Down) {
                            return (2, 3);
                        }
                    }

                    if ((cell.Directions & Directions.Left) == Directions.Left) {
                        return (2, 2);
                    }

                    throw new ArgumentOutOfRangeException();
                }
                case 3: {
                    if ((cell.Directions & Directions.Up) != Directions.Up) {
                        return (1, 0);
                    }

                    if ((cell.Directions & Directions.Right) != Directions.Right) {
                        return (1, 3);
                    }

                    if ((cell.Directions & Directions.Down) != Directions.Down) {
                        return (1, 2);
                    }

                    if ((cell.Directions & Directions.Left) != Directions.Left) {
                        return (1, 1);
                    }

                    throw new ArgumentOutOfRangeException();
                }
                case 4:  return (0, 0);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void ClearRooms() {
            _grid.Clear();
        }

        private static void SetupLogger() {
            var config = new LoggerConfiguration()
                    .WriteTo.Console()
#if DEBUG
                    .MinimumLevel.Debug()
#else
                    .MinimumLevel.Warning()
#endif
                ;

            Log.Logger = config.CreateLogger();
        }
    }
}
