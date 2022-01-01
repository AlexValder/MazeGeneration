using System;
using System.Collections.Generic;
using System.Linq;
using Demonomania.Scripts.MazeGen.Algo;
using Demonomania.Scripts.MazeGen.Mask;
using Demonomania.Scripts.MazeGen.Util;
using Godot;

namespace Demonomania.Scripts.MazeGen {
    public class AlgorithmManager {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type> {
            ["Randomized"]       = typeof(RandomMaze),
            ["Binary Tree"]      = typeof(BinaryTree),
            ["Sidewinder"]       = typeof(Sidewinder),
            ["Aldous-Broder"]    = typeof(AldousBroder),
            ["Wilson"]           = typeof(Wilson),
            ["Hunt&Kill"]        = typeof(HuntAndKill),
            ["Rec. Backtracker"] = typeof(RecursiveBacktracker),
            ["Kruskal"]          = typeof(Kruskal),
            ["Prim"]             = typeof(Prim),
        };

        public AlgorithmManager(OptionButton button) {
            var array = new Godot.Collections.Array<string[]>();

            foreach (var name in _types.Keys) {
                if (Attribute.IsDefined(_types[name], typeof(MaskableAttribute))) {
                    array.Add(new[] {name, "true"});
                } else {
                    array.Add(new[] {name});
                }
            }

            button?.Call("initialize", array);
        }

        public AbstractMazeGen GetAlgorithm(AlgoParams @params) {
            if (!_types.ContainsKey(@params.Name)) {
                throw new ArgumentOutOfRangeException($"Unknown algo: {@params.Name}");
            }

            var type = _types[@params.Name];

            Grid grid;
            if (@params.Mask && Attribute.IsDefined(type, typeof(MaskableAttribute))) {
                grid = new MaskedGrid(
                    @params.Width,
                    @params.Height,
                    MaskCreator.CornerEastNorth(@params.Width, @params.Height)
                );
            } else {
                grid = new Grid(@params.Width, @params.Height);
            }

            return Activator.CreateInstance(type, grid, @params.Seed) as AbstractMazeGen;
        }
    }
}
