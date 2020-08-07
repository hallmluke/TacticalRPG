using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int width;
    [SerializeField] int length;

    Color selectedTileColor = Color.blue;
    Color defaultTileColor = Color.white;
    public Dictionary<Coord, Tile> tiles = new Dictionary<Coord, Tile>();
    Coord[] directions = { new Coord(1, 0), new Coord(-1, 0), new Coord(0, 1), new Coord(0, -1) };

    public void Load (MapData data) {
      for (int i=0; i < data.tiles.Count; ++i) {
        GameObject instance = Instantiate(tilePrefab) as GameObject;
        instance.transform.parent = transform;
        Tile t = instance.GetComponent<Tile>();
        t.Load(data.tiles[i]);
        tiles.Add(t.coord, t);
      }
    }

    public Vector3 CoordToPosition(int x, int y) {
        return tiles[new Coord(x, y)].center;
    }

    public Vector3 CoordToPosition(Coord coord) {
      return tiles[coord].center;
    }

    public Tile GetTileFromCoord(Coord coord) {
      if(validCoord(coord)) {
        return tiles[coord];
      }

      return null;
    }
    public bool validCoord(Coord coord) {
        return coord.x >= 0 && coord.x < width && coord.y >= 0 && coord.y < length && tiles.ContainsKey(coord);
    }

    public HashSet<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
      HashSet<Tile> result = new HashSet<Tile>();
      result.Add(start);

      ClearSearch();

      Queue<Tile> toVisit = new Queue<Tile>();

      start.distance = 0;
      toVisit.Enqueue(start);
      
      while (toVisit.Count > 0)
      {
          Tile t = toVisit.Dequeue();

          for(int i=0; i<directions.Length; i++) {

            Tile next = GetTileFromCoord(t.coord + directions[i]);

            if (next == null || next.distance <= t.distance + 1) {
              continue;
            }

            if (addTile(t, next))
            {
                next.distance = t.distance + next.movementCost;
                next.parent = t;
                toVisit.Enqueue(next);
                result.Add(next);
            }
          }
      }

      return result;
    }

    void ClearSearch ()
    {
      foreach (Tile t in tiles.Values)
      {
        t.parent = null;
        t.distance = int.MaxValue;
      }
    }
    
    public void SelectTiles (HashSet<Tile> tiles)
    {
      foreach (Tile tile in tiles) {
        tile.GetComponent<Renderer>().material = tile.movable;
      }
    }
    public void DeSelectTiles (HashSet<Tile> tiles)
    {
      foreach (Tile tile in tiles) {
        tile.GetComponent<Renderer>().material = tile.unselected;
      }
    }

    public void DeSelectTiles(List<Tile> tiles) {
      foreach (Tile tile in tiles) {
        tile.GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
      }
    }
}
