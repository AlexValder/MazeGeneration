﻿namespace Demonomania.Scripts.MazeGen.Util {
    public class Cell {
        public int X { get; set; }
        public int Y { get; set; }
        public Directions Directions { get; set; }

        public bool Enabled { get; set; } = true;
        public Cell(Directions directions) {
            Directions = directions;
        }
    }
}
