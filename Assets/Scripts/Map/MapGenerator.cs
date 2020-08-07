using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class MapGenerator : MonoBehaviour
{
    //public Coord mapSize;
    //public float tileSize;
    //public Transform defaultTilePrefab;
    //public Transform[,] tileMap;

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject selectionPrefab;

    [SerializeField] int width;
    [SerializeField] int length;
    [SerializeField] int height;
    [SerializeField] Coord pos;
    public MapData mapData;

    /*Transform marker
    {
        get 
        {
            if(_marker == null) {
                GameObject instance = Instantiate(selectionPrefab) as GameObject;
                _marker = instance.transform;
            }

            return _marker;
        }
    }*/
    Transform _marker;
    [SerializeField] CoordTileDictionary tiles;// = new Dictionary<Coord, Tile>();

    void Awake() {
        GenerateMap();
    }

    public void GenerateMap() {

        tiles = new CoordTileDictionary();

        //tileMap = new Transform[mapSize.x, mapSize.y];
        // Create map holder object
        string holderName = "Generated Map";
        if(transform.Find(holderName)) {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for(int i=0; i < width; i++) {
            for(int j=0; j < length; j++) {
                Coord initPos = new Coord(i, j);
                //Vector3 tilePosition = CoordToPosition(i, j);
                //tileMap[i,j] = Instantiate(defaultTilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                GameObject newTileObject = Instantiate(tilePrefab, Vector3.zero, Quaternion.Euler(Vector3.up));
                tiles[initPos] = newTileObject.GetComponent<Tile>();
                Tile newTile = tiles[initPos];
                newTile.height = 1;
                newTile.coord = initPos;

                newTile.Match();
                newTileObject.transform.parent = mapHolder;
                newTileObject.transform.name = i + ", " + j;
                
            }
        }
    }

    public Vector3 CoordToPosition(int x, int y) {
        return tiles[new Coord(x, y)].center;
    }

    public Vector3 CoordToPosition(Coord coord) {
        return tiles[coord].center;
    }

    public Tile GetTileFromCoord(Coord coord) {
        return tiles[coord];
    }

    /*public Tile GetTileFromPosition(Vector3 position) {
        int x = Mathf.RoundToInt(position.x / tileSize + (mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (mapSize.y - 1) / 2f);

        x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
        return tileMap[x, y];
    }*/

    public bool validCoord(Coord coord) {
        return coord.x >= 0 && coord.x < width && coord.y >= 0 && coord.y < length;
    }

    public void ElevateArea ()
    {
        Rect r = RandomRect();
        ElevateRect(r);
    }
    public void LowerArea ()
    {
        Rect r = RandomRect();
        LowerRect(r);
    }

    Rect RandomRect ()
{
  int x = UnityEngine.Random.Range(0, width);
  int y = UnityEngine.Random.Range(0, length);
  int w = UnityEngine.Random.Range(1, width - x + 1);
  int h = UnityEngine.Random.Range(1, length - y + 1);
  return new Rect(x, y, w, h);
}

void ElevateRect (Rect rect)
{
  for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
  {
    for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
    {
      Coord p = new Coord(x, y);
      ElevateSingle(p);
    }
  }
}
void LowerRect (Rect rect)
{
  for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
  {
    for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
    {
      Coord p = new Coord(x, y);
      LowerSingle(p);
    }
  }
}

Tile Create ()
{
  GameObject instance = Instantiate(tilePrefab) as GameObject;
  instance.transform.parent = transform;
  return instance.GetComponent<Tile>();
}
Tile GetOrCreate (Coord p)
{
  if (tiles.ContainsKey(p))
    return tiles[p];
  
  Tile t = Create();
  t.Load(p, 0);
  tiles.Add(p, t);
  
  return t;
}
public void ElevateSingle (Coord p)
{
  Tile t = GetOrCreate(p);
  if (t.height < height)
    t.Elevate();
}

public void LowerSingle (Coord p)
{
  if (!tiles.ContainsKey(p))
    return;
  
  Tile t = tiles[p];
  t.Lower();
  
  if (t.height <= 0)
  {
    tiles.Remove(p);
    DestroyImmediate(t.gameObject);
  }
}

public void Elevate ()
{
  ElevateSingle(pos);
}
public void Lower ()
{
  LowerSingle(pos);
}

/*public void UpdateMarker ()
{
  Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
  marker.localPosition = t != null ? t.center : new Vector3(pos.x, 1, pos.y);
}*/

public void Clear ()
{
  for (int i = transform.childCount - 1; i >= 0; --i)
    DestroyImmediate(transform.GetChild(i).gameObject);
  tiles.Clear();
}

public void Save ()
{
  string filePath = Application.dataPath + "/Resources/Maps";
  if (!Directory.Exists(filePath))
    CreateSaveDirectory ();
  
  MapData board = ScriptableObject.CreateInstance<MapData>();
  board.tiles = new List<Vector3>( tiles.Count );
  foreach (Tile t in tiles.Values)
    board.tiles.Add( new Vector3(t.coord.x, t.height, t.coord.y) );
  
  string fileName = string.Format("Assets/Resources/Maps/{1}.asset", filePath, name);
  AssetDatabase.CreateAsset(board, fileName);
}
void CreateSaveDirectory ()
{
  string filePath = Application.dataPath + "/Resources";
  if (!Directory.Exists(filePath))
    AssetDatabase.CreateFolder("Assets", "Resources");
  filePath += "/Maps";
  if (!Directory.Exists(filePath))
    AssetDatabase.CreateFolder("Assets/Resources", "Maps");
  AssetDatabase.Refresh();
}

public void Load ()
{
  Clear();
  if (mapData == null)
    return;
  foreach (Vector3 v in mapData.tiles)
  {
    Tile t = Create();
    t.Load(v);
    tiles.Add(t.coord, t);
  }
}
}
