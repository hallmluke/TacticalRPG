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
    public Team team;

    public Job job;

    // Base Stats
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseSpAttack;
    public int baseSpDefense;
    public int baseSpeed;
    public int moveRange;


    // Calculated Stats
    public int maxHealth;
    public int currentHealth;
    public int totalAttack;
    public int totalDefense;
    public int totalSpAttack;
    public int totalSpDefense;
    public int totalSpeed;

    public float moveSpeed = 3f;
    public bool moving = false;
    public bool acted = false;

    Renderer unitRenderer;
    public Material startingMaterial;

    Material waitMaterial;



    
    void Awake() {
        movement = gameObject.AddComponent<Movement>();
        team.unitsInTeam.Add(this);

        unitRenderer = transform.GetComponent<Renderer>();
        startingMaterial = unitRenderer.material;
        waitMaterial = new Material(unitRenderer.material);
        float percentDarker = .70f;
        waitMaterial.color = new Color(waitMaterial.color.r * percentDarker, waitMaterial.color.g * percentDarker, waitMaterial.color.b * percentDarker);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetWorldPositionFromMapPosition();
        currentTile = map.GetTileFromCoord(position);
        currentTile.OccupyTile(this);

        RecalculateStats();

        currentHealth = maxHealth;
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

    public void CancelMove(Tile originalTile) {
        
        currentTile.LeaveTile();

        position = originalTile.coord;

        currentTile = originalTile;

        currentTile.OccupyTile(this);

        SetWorldPositionFromMapPosition();

    }

    public void ExecuteAction(Tile target, Action action) {
        Unit targetUnit = target.GetUnitOnTile();

        action.Execute(this, targetUnit);
    }

    public void ReceiveDamage(int damage) {

        currentHealth = Mathf.Max(0, currentHealth - Mathf.Max(0, damage));

        if(currentHealth == 0) {
            currentTile.LeaveTile();
            team.RemoveUnitFromTeam(this);

            Destroy(gameObject);
        }
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

    public void NewTurn() {
        acted = false;
        unitRenderer.material = startingMaterial;
    }
    public void EndTurn() {
        acted = true;
        unitRenderer.material = waitMaterial;
        team.RemoveUnitFromLeftToMove(this);
    }

    public void RecalculateStats() {

        maxHealth = Mathf.RoundToInt(baseHealth * job.healthModifier);
        totalAttack = Mathf.RoundToInt(baseAttack * job.attackModifier);
        totalDefense = Mathf.RoundToInt(baseDefense * job.defenseModifier);
        totalSpAttack = Mathf.RoundToInt(baseSpAttack * job.spAttackModifier);
        totalSpDefense = Mathf.RoundToInt(baseSpDefense * job.spDefenseModifier);
        totalSpeed = Mathf.RoundToInt(baseSpeed * job.speedModifier);

    }

}
