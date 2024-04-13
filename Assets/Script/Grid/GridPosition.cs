
using System;

public  struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return "x : " + x + ", z : " + z;
    }

    /// Overloads the equality operator to compare two GridPosition instances for equality.
    public static bool operator ==(GridPosition a , GridPosition b)
    {
        return a.x==b.x && a.z==b.z;
    }

    /// Overloads the inequality operator to compare two GridPosition instances for inequality.
    public static bool operator !=(GridPosition a , GridPosition b)
    {
        return !(a == b);
    }

    /// Overloads the addition operator to add two GridPosition instances.
    public static GridPosition operator+(GridPosition a , GridPosition b)
    {
        return new GridPosition(a.x+b.x, a.z+b.z);
    }

    // Overloads the subtraction operator to subtract one GridPosition instance from another.
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }

}