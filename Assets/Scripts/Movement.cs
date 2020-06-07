using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Map map;
    Coord[] directions = { new Coord(1, 0), new Coord(-1, 0), new Coord(0, 1), new Coord(0, -1) };

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

        if(0 <= maxRange && 0 >= minRange) {
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
            if(tile.GetUnitOnTile() == null) {
                result.Add(tile);
                tile.isMovable = true;
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
                tile.isAttackable = true;
            } 
        }

        return result;
    }

    void Awake() {
        map = FindObjectOfType<Map>();
    }

}
