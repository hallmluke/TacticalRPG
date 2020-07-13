using UnityEngine;
using System.Collections;
public class FlyMovement : Movement
{
    public override IEnumerator Traverse(Tile tile)
    {
        print("Calculate distance");
        // Store the distance between the start tile and target tile
        float dist = Mathf.Sqrt(Mathf.Pow(tile.coord.x - unit.currentTile.coord.x, 2) + Mathf.Pow(tile.coord.y - unit.currentTile.coord.y, 2));
        unit.Place(tile);
        // Fly high enough not to clip through any ground tiles
        print("Fly up");
        float y = Tile.heightToWidth * 10;
        float duration = (y - jumper.position.y) * 0.5f;
        Tweener tweener = jumper.MoveToLocal(new Vector3(0, y, 0), duration, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
        // Turn to face the general direction
        print("Turn");
        Directions dir;
        Vector3 toTile = (tile.center - transform.position);
        if (Mathf.Abs(toTile.x) > Mathf.Abs(toTile.z))
            dir = toTile.x > 0 ? Directions.East : Directions.West;
        else
            dir = toTile.z > 0 ? Directions.North : Directions.South;
        yield return StartCoroutine(Turn(dir));
        // Move to the correct position
        print("move");
        duration = dist * 0.5f;
        tweener = transform.MoveTo(tile.center, duration, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
        // Land
        print("land");
        duration = (y - tile.center.y) * 0.5f;
        tweener = jumper.MoveToLocal(Vector3.zero, 0.5f, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
    }
}