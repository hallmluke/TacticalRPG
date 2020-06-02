using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    public Map map;
    public Coord position;
    public Tile currentTile;
    public Movement movement;
    public string name;
    public int maxHealth;
    public int currentHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;
    public int moveRange;
    public float moveSpeed = 3f;

    
    void Awake() {
        movement = gameObject.AddComponent<Movement>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SetWorldPositionFromMapPosition();
        currentTile = map.GetTileFromCoord(position);
        currentTile.OccupyTile(this);

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Coord coord, List<Tile> path) {
        currentTile.LeaveTile();
        position = coord;
        StartCoroutine(FollowPath(path));
        //SetWorldPositionFromMapPosition();
        currentTile = map.GetTileFromCoord(position);
        currentTile.OccupyTile(this);
    }


    IEnumerator FollowPath(List<Tile> path) {
        for(int i = path.Count - 1; i >= 0; i--) {
            Tile waypoint = path[i];
            yield return StartCoroutine(MoveToTile(waypoint));
        }
    }

    IEnumerator MoveToTile(Tile destination) {
        while(transform.position != destination.transform.position + Vector3.up) {
            
            transform.position = Vector3.MoveTowards(transform.position, destination.transform.position + Vector3.up, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void SetWorldPositionFromMapPosition() {
        Vector3 worldPosition = map.CoordToPosition(position.x, position.y) + Vector3.up;
        transform.position = worldPosition;
    }

    public HashSet<Coord> FindMovementOptions() {
        return movement.CalculateMovementOptions(position, moveRange);
    }
}
