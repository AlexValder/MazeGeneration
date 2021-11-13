using System;
using System.Runtime.ExceptionServices;
using Godot;

namespace Demonomania.Scripts.MazeGen {
    public struct Edge {
        public Cell First { get; set; }
        public Cell Second { get; set; }

        public static bool operator<(Edge left, Edge right) =>
            left.First.X < right.First.X ||
            left.First.Y < right.First.Y ||
            left.Second.X < right.Second.X ||
            left.Second.Y < right.Second.Y;

        public static bool operator>(Edge left, Edge right) => right > left;
    }
}
