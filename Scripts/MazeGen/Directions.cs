using System;

namespace Demonomania.Scripts.MazeGen {
    [Flags]
    public enum Directions {
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
    }
}
