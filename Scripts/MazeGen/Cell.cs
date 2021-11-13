using Godot;

namespace Demonomania.Scripts.MazeGen {
    public class Cell {
        public int X { get; set; }
        public int Y { get; set; }
        public Directions Directions { get; set; }

        public Cell(Directions directions) {
            Directions = directions;
        }
    }
}
