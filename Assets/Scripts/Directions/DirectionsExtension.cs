using UnityEngine;
using System.Collections;
public static class DirectionsExtensions
{
    public static Directions GetDirection(this Tile t1, Tile t2)
    {
        if (t1.coord.y < t2.coord.y)
            return Directions.North;
        if (t1.coord.x < t2.coord.x)
            return Directions.East;
        if (t1.coord.y > t2.coord.y)
            return Directions.South;
        return Directions.West;
    }

    public static Directions GetDirection(this Coord c)
    {
        if (c.y > 0)
            return Directions.North;
        if (c.x > 0)
            return Directions.East;
        if (c.y < 0)
            return Directions.South;
        return Directions.West;
    }
    public static Vector3 ToEuler(this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}