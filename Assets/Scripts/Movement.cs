using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Map map;
    Coord[] directions = { new Coord(1, 0), new Coord(-1, 0), new Coord(0, 1), new Coord(0, -1) };

    struct MoveTile {
        public Coord position;
        public int remainingMov;

        public MoveTile(Coord pos, int mov) {
            position = pos;
            remainingMov = mov;
        }
    }

    public HashSet<Coord> CalculateMovementOptions(Coord currentPosition, int moveRange) {

        HashSet<Coord> visitedCoords = new HashSet<Coord>();
        Queue<MoveTile> toVisit = new Queue<MoveTile>();
        MoveTile startPosition = new MoveTile(currentPosition, moveRange);

        visitedCoords.Add(currentPosition);
        toVisit.Enqueue(startPosition);

        while(toVisit.Count != 0) {

            MoveTile currentTile = toVisit.Dequeue();
            visitedCoords.Add(currentTile.position);

            for(int i=0; i<directions.Length; i++) {

                Coord targetCoord = new Coord(currentTile.position.x + directions[i].x, currentTile.position.y + directions[i].y);

                if(map.validCoord(targetCoord)) {

                    Tile tileAtTargetCoord = map.tileMap[targetCoord.x, targetCoord.y].GetComponent<Tile>();

                    if(currentTile.remainingMov - 1 >= 0 && !visitedCoords.Contains(targetCoord) && tileAtTargetCoord.GetUnitOnTile() == null) {

                        toVisit.Enqueue(new MoveTile(targetCoord, currentTile.remainingMov - 1));
                        tileAtTargetCoord.parent = map.GetTileFromCoord(currentTile.position);

                    }

                }
            }

        }

        foreach(Coord movable in visitedCoords) {
            Tile tile = map.tileMap[movable.x, movable.y].GetComponent<Tile>();
            
            tile.isMovable = true;
        }

        return visitedCoords;
    }

    void Awake() {
        map = FindObjectOfType<Map>();
    }

}
