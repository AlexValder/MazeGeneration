using System;
using System.Diagnostics;
using Demonomania.Scripts.MazeGen.Algo;

namespace Demonomania.Scripts.MazeGen {
    public static class AlgorithmManager {
        public static AbstractMazeGen GetAlgorithm(string name, int width, int height, int? seed = null) {
            switch (name) {
                case "Randomized":
                    return new RandomMaze(
                        width,
                        height,
                        seed
                    );
                case "Binary Tree":
                    return new BinaryTree(
                        width,
                        height,
                        seed
                    );
                case "Kruskal":
                    return new Kruskal(
                        width,
                        height,
                        seed
                    );
                case "Hunt&Kill":
                    return new HuntAndKill(
                        width,
                        height,
                        seed
                    );
                case "Prim":
                    return new Prim(
                        width,
                        height,
                        seed
                    );
                default:
                    throw new ArgumentOutOfRangeException($"Unknown algo: name");
            }
        }
    }
}
