using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static Coord operator +(Coord a, Coord b)
    {
        return new Coord(a.x + b.x, a.y + b.y);
    }
    public static Coord operator -(Coord p1, Coord p2)
    {
        return new Coord(p1.x - p2.x, p1.y - p2.y);
    }
    public static bool operator ==(Coord c1, Coord c2)
    {
        return c1.x == c2.x && c1.y == c2.y;
    }

    public static bool operator !=(Coord c1, Coord c2)
    {
        return !(c1 == c2);
    }

    public override bool Equals(object obj)
    {
        if (obj is Coord)
        {
            Coord p = (Coord)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }
    
    public bool Equals(Coord p)
    {
        return x == p.x && y == p.y;
    }
    public override int GetHashCode()
    {
        return x ^ y;
    }

    public override string ToString ()
    {
        return string.Format ("({0},{1})", x, y);
    }
}