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
    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;
    public int moveRange;

    int currentHealth;
    
    void Awake() {
        movement = gameObject.AddComponent<Movement>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SetWorldPositionFromMapPosition();
        currentTile = map.GetTileFromCoord(position);
        currentTile.OccupyTile(this);
        //movement = gameObject.AddComponent<Movement>();
        //movement.CalculateMovementOptions(position, moveRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Coord coord) {
        currentTile.LeaveTile();
        position = coord;
        SetWorldPositionFromMapPosition();
        currentTile = map.GetTileFromCoord(position);
        currentTile.OccupyTile(this);
    }

    public void SetWorldPositionFromMapPosition() {
        Vector3 worldPosition = map.CoordToPosition(position.x, position.y) + Vector3.up;
        transform.position = worldPosition;
    }

    public HashSet<Coord> FindMovementOptions() {
        return movement.CalculateMovementOptions(position, moveRange);
    }
}
