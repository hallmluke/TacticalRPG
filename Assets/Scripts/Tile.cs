using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject visualPrefab;
    public float movementCost = 1;
    public Coord coord;

    public Material defaultMaterial;
    public Material movableMaterial;

    Renderer tileRenderer;
    
    Unit unitOnTile;
    Tile parent;
    public bool isMovable;
    bool isSelected;

    void Start() {

        tileRenderer = transform.GetComponent<Renderer>();
        defaultMaterial = tileRenderer.sharedMaterial;
        movableMaterial = new Material(tileRenderer.sharedMaterial);

        movableMaterial.color = Color.blue;
    }
    void Update() {

        if(isMovable) {

            tileRenderer.material = movableMaterial;

        } else {

            tileRenderer.material = defaultMaterial;

        }
    }
    public void OccupyTile(Unit unit) {
        unitOnTile = unit;
    }

    public void LeaveTile() {
        unitOnTile = null;
    }

    public Unit GetUnitOnTile() {
        return unitOnTile;
    }
}
