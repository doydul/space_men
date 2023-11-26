using System;

public struct Point {
    
    public int x;
    public int y;

    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public int ManhattanDistance(Point other) {
        return (int)Math.Abs(x - other.x) + (int)Math.Abs(y - other.y);
    }

    public override bool Equals(object obj) => this == (Point)obj;
    public override int GetHashCode() => (x, y).GetHashCode();
    public override string ToString() => "Point(" + x + ", " + y + ")";

    public static bool operator ==(Point lhs, Point rhs) {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Point lhs, Point rhs) => !(lhs == rhs);
}