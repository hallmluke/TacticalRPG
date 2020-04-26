using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour
{
    public Coord mapSize;
    public float tileSize;
    public Transform defaultTilePrefab;
    public Transform[,] tileMap; 

    void Awake() {
        GenerateMap();
    }

    public Coord mapCenter
    {
        get
        {
            return new Coord(mapSize.x / 2, mapSize.y / 2);
        }
    }

    public void GenerateMap() {

        tileMap = new Transform[mapSize.x, mapSize.y];
        // Create map holder object
        string holderName = "Generated Map";
        if(transform.Find(holderName)) {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for(int i=0; i < mapSize.x; i++) {
            for(int j=0; j < mapSize.y; j++) {

                Vector3 tilePosition = CoordToPosition(i, j);
                tileMap[i,j] = Instantiate(defaultTilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                Tile newTile = tileMap[i,j].GetComponent<Tile>();
                newTile.coord = new Coord(i, j);

                tileMap[i,j].localScale = Vector3.one * tileSize;
                tileMap[i,j].parent = mapHolder;
                tileMap[i,j].name = i + ", " + j;
                
            }
        }
    }

    public Vector3 CoordToPosition(int x, int y) {
        return new Vector3(-mapSize.x / 2f + 0.5f + x, 0, -mapSize.y / 2f + 0.5f + y) * tileSize;
    }

    public Tile GetTileFromCoord(Coord coord) {
        return tileMap[coord.x, coord.y].GetComponent<Tile>();
    }

    public Transform GetTileFromPosition(Vector3 position) {
        int x = Mathf.RoundToInt(position.x / tileSize + (mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (mapSize.y - 1) / 2f);

        x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
        return tileMap[x, y];
    }

    public bool validCoord(Coord coord) {
        return coord.x >= 0 && coord.x < mapSize.x && coord.y >= 0 && coord.y < mapSize.y;
    }

}
