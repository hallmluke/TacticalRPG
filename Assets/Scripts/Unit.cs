using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    public Map map;
    public Coord position;
    public Tile currentTile;
    public Directions dir;
    public Movement movement;
    public string displayName;
    public Team team;

    public Job job;

    public float moveSpeed = 3f;

    Renderer unitRenderer;
    public Material startingMaterial;

    Material waitMaterial;



    
    void Awake() {
        map = FindObjectOfType<Map>();
       // movement = gameObject.AddComponent<Movement>();
        // TODO: Add back in teams
       // team.unitsInTeam.Add(this);

        unitRenderer = transform.GetComponent<Renderer>();
        startingMaterial = unitRenderer.material;
        waitMaterial = new Material(unitRenderer.material);
        float percentDarker = .70f;
        waitMaterial.color = new Color(waitMaterial.color.r * percentDarker, waitMaterial.color.g * percentDarker, waitMaterial.color.b * percentDarker);
    }
    // Start is called before the first frame update
    void Start()
    {
 
    }

    public void Place (Tile target)
    {
    // Make sure old tile location is not still pointing to this unit
    if (currentTile != null && currentTile.GetUnitOnTile() == this)
      currentTile.LeaveTile();
    
    // Link unit and tile references
    currentTile = target;
    
    if (target != null)
      target.OccupyTile(this);
  }
    public void SetWorldPositionFromMapPosition() {
        Vector3 worldPosition = currentTile.center + Vector3.up * currentTile.height * Tile.heightToWidth;
        transform.position = worldPosition;
        transform.localEulerAngles = dir.ToEuler();
    }
}
