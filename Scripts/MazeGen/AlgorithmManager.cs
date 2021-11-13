using System;

namespace Demonomania.Scripts.MazeGen {
    public static class AlgorithmManager {
        public static AbstractMazeGen GetAlgorithm(string name, int count, int? seed = null) {
            switch (name) {
                case "Randomized":
                    return new RandomMaze(
                        count,
                        count,
                        seed
                    );
                case "Kruskal":
                    return new Kruskal(
                        count,
                        count,
                        seed
                    );
                default:
                    throw new ArgumentOutOfRangeException($"Unknown algo: name");
            }
        }
    }
}
