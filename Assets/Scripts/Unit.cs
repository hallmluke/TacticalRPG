using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    public Map map;
    public Coord position;
    public Tile currentTile;
    public Movement movement;
    public string displayName;
    public int maxHealth;
    public int currentHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;
    public int moveRange;
    public float moveSpeed = 3f;
    public bool moving = false;



    
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

        if(path == null || path.Count == 0) {
            print("Invalid path passed to Move");
        }

        moving = true;
        currentTile.LeaveTile();
        position = coord;

        List<Tile> pathCopy = new List<Tile>(path);

        StartCoroutine(FollowPath(pathCopy));

        currentTile = map.GetTileFromCoord(position);
        currentTile.OccupyTile(this);
    }

    public void Attack(Tile target) {
        Unit targetUnit = target.GetUnitOnTile();

        targetUnit.currentHealth -= Mathf.Max(0, baseAttack - targetUnit.baseDefense);
    }


    IEnumerator FollowPath(List<Tile> path) {
        for(int i = path.Count - 1; i >= 0; i--) {
            Tile waypoint = path[i];
            yield return StartCoroutine(MoveToTile(waypoint));
        }
        moving = false;
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

    public HashSet<Tile> FindMovementOptions() {
        return movement.GetMovementTiles(position, moveRange);
    }

    public HashSet<Tile> FindAttackTargets() {
        return movement.GetAttackTargets(position, 1, 1);
    }
}
