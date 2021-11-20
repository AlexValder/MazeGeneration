using System;
using System.Runtime.ExceptionServices;
using Godot;

namespace Demonomania.Scripts.MazeGen {
    public struct Edge: IEquatable<Edge> {
        public Cell First { get; }
        public Cell Second { get; }

        public static bool operator<(Edge left, Edge right) =>
            left.First.X < right.First.X ||
            left.First.Y < right.First.Y ||
            left.Second.X < right.Second.X ||
            left.Second.Y < right.Second.Y;

        public static bool operator>(Edge left, Edge right) => right > left;

        public bool Equals(Edge other) {
            return Equals(First, other.First) && Equals(Second, other.Second);
        }

        public override bool Equals(object obj) {
            return obj is Edge other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return ((First?.GetHashCode() ?? 0) * 397) ^ (Second?.GetHashCode() ?? 0);
            }
        }
    }
}
