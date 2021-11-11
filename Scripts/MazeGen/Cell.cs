using Godot;

namespace Demonomania.Scripts.MazeGen {
    public class Cell {
        public Cell Parent { get; }

        public Cell(Cell parent, Directions directions) {
            Parent     = parent;
            Directions = directions;
        }
        public Cell(Directions directions) {
            Parent     = null;
            Directions = directions;
        }

        public Directions Directions { get; set; }

        public static Cell HighestCell(Cell cell) {
            while (true) {
                if (cell.Parent == null) return cell;
                cell = cell.Parent;
            }
        }
    }
}
