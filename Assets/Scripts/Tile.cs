using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float movementCost = 1;
    public Coord coord;

    public Material defaultMaterial;
    public Material movableMaterial;
    public Material pathMaterial;
    public Material attackMaterial;

    Renderer tileRenderer;
    
    Unit unitOnTile;
    public Tile parent;
    public bool isMovable;
    public bool isAttackable;
    public bool inPath;

    void Start() {

        tileRenderer = transform.GetComponent<Renderer>();
        defaultMaterial = tileRenderer.sharedMaterial;
        movableMaterial = new Material(tileRenderer.sharedMaterial);
        pathMaterial = new Material(tileRenderer.sharedMaterial);
        attackMaterial = new Material(tileRenderer.sharedMaterial);

        movableMaterial.color = Color.blue;
        pathMaterial.color = Color.green;
        attackMaterial.color = Color.red;


    }
    void Update() {

        if(inPath) {
            tileRenderer.material = pathMaterial;

        } else if(isMovable) {

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
