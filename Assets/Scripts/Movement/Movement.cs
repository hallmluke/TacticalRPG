using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range { get { return stats[StatTypes.MOV]; }}
    public int jumpHeight { get { return stats[StatTypes.JMP]; }}
    protected Stats stats;
    protected Unit unit;
    protected Transform jumper;
    Map map;
    Coord[] directions = { new Coord(1, 0), new Coord(-1, 0), new Coord(0, 1), new Coord(0, -1) };


    protected virtual void Awake() {
        map = FindObjectOfType<Map>();
        unit = GetComponent<Unit>();
        jumper = transform.FindChild("Jumper");
    }

    protected virtual void Start ()
    {
        stats = GetComponent<Stats>();
    }
/*
    struct MoveTile {
        public Coord position;
        public int distance;

        public MoveTile(Coord pos, int mov) {
            position = pos;
            distance = mov;
        }
    }

    public HashSet<Tile> GetTilesInRange(Coord currentPosition, int minRange, int maxRange, bool ignoreObstacles = false) {

        HashSet<Coord> visitedCoords = new HashSet<Coord>();
        Queue<MoveTile> toVisit = new Queue<MoveTile>();
        HashSet<Tile> result = new HashSet<Tile>();

        MoveTile startPosition = new MoveTile(currentPosition, 0);

        if(0 <= maxRange && 0 == minRange) {
            Tile tileInRange = map.GetTileFromCoord(currentPosition);

            result.Add(tileInRange);
        }

        visitedCoords.Add(currentPosition);
        toVisit.Enqueue(startPosition);

        while(toVisit.Count != 0) {

            MoveTile currentTile = toVisit.Dequeue();
            visitedCoords.Add(currentTile.position);

            if(currentTile.distance <= maxRange && currentTile.distance >= minRange) {
                Tile tileInRange = map.GetTileFromCoord(currentTile.position);

                result.Add(tileInRange);
            }

            for(int i=0; i<directions.Length; i++) {

                Coord targetCoord = new Coord(currentTile.position.x + directions[i].x, currentTile.position.y + directions[i].y);

                if(map.validCoord(targetCoord)) {

                    Tile tileAtTargetCoord = map.GetTileFromCoord(targetCoord);

                    if(maxRange >= currentTile.distance + tileAtTargetCoord.movementCost && !visitedCoords.Contains(targetCoord)) {

                        if(ignoreObstacles || (!ignoreObstacles && tileAtTargetCoord.GetUnitOnTile() == null)) {
                            toVisit.Enqueue(new MoveTile(targetCoord, currentTile.distance + tileAtTargetCoord.movementCost));
                            tileAtTargetCoord.parent = map.GetTileFromCoord(currentTile.position);
                        }
                    }

                }
            }

        }


        return result;
    }

    public HashSet<Tile> GetMovementTiles(Coord position, int maxRange) {
        HashSet<Tile> tilesInRange = GetTilesInRange(position, 0, maxRange);
        HashSet<Tile> result = new HashSet<Tile>();

        foreach(Tile tile in tilesInRange) {
            if(tile.GetUnitOnTile() == null || tile.GetUnitOnTile() == transform.GetComponent<Unit>()) {
                result.Add(tile);
                //tile.isMovable = true;
                tile.SetMaterialToMovable();
            } 
        }

        return result;
    }

    public HashSet<Tile> GetAttackTargets(Coord position, int minRange, int maxRange) {
        HashSet<Tile> tilesInRange = GetTilesInRange(position, minRange, maxRange, true);
        HashSet<Tile> result = new HashSet<Tile>();

        foreach(Tile tile in tilesInRange) {
            if(tile.GetUnitOnTile() != null) {
                result.Add(tile);
                //tile.isAttackable = true;
                tile.SetMaterialToAttackable();
            } 
        }

        return result;
    }*/
    public virtual HashSet<Tile> GetTilesInRange (Map map)
    {
        HashSet<Tile> result = map.Search( unit.currentTile, ExpandSearch );
        return Filter(result);
    }

    protected virtual bool ExpandSearch (Tile from, Tile to)
    {
        return (from.distance + to.movementCost) <= range;
    }

    protected virtual HashSet<Tile> Filter (HashSet<Tile> tiles)
    {
        HashSet<Tile> result = new HashSet<Tile>();

        foreach (Tile tile in tiles) {
            if (tile.GetUnitOnTile() == null) {
                result.Add(tile);
            }
        }

        return result;
    }

    public abstract IEnumerator Traverse (Tile tile);

    public virtual IEnumerator Turn (Directions dir) {
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);
  
        // When rotating between North and West, we must make an exception so it looks like the unit
        // rotates the most efficient way (since 0 and 360 are treated the same)
        if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
            t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
            t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        unit.dir = dir;
  
        while (t != null)
            yield return null;
        }

}
