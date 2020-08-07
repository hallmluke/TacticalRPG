using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public const float heightToWidth = 0.75f;
    public int movementCost = 1;
    public Coord coord;
    public int height;

    public Vector3 center { get { return new Vector3(coord.x, height, coord.y); }}

/*
    public Material defaultMaterial;
    public Material movableMaterial;
    public Material pathMaterial;
    public Material attackMaterial;

    Renderer tileRenderer;
    */
    Unit unitOnTile;
    public Tile parent;
    public int distance;

    public Material unselected;
    public Material movable;
    public Material target;
 

    void Start() {

        /*tileRenderer = transform.GetComponent<Renderer>();
        defaultMaterial = tileRenderer.sharedMaterial;
        movableMaterial = new Material(tileRenderer.sharedMaterial);
        pathMaterial = new Material(tileRenderer.sharedMaterial);
        attackMaterial = new Material(tileRenderer.sharedMaterial);

        movableMaterial.color = Color.blue;
        pathMaterial.color = Color.green;
        attackMaterial.color = Color.red;*/


    }

    /*public void SetMaterialToPath() {
        tileRenderer.material = pathMaterial;
    }

    public void SetMaterialToMovable() {
        tileRenderer.material = movableMaterial;
    }

    public void SetMaterialToAttackable() {
        tileRenderer.material = attackMaterial;
    }

    public void ResetMaterial() {
        tileRenderer.material = defaultMaterial;
    }*/

    public void OccupyTile(Unit unit) {
        unitOnTile = unit;
    }

    public void LeaveTile() {
        unitOnTile = null;
    }

    public Unit GetUnitOnTile() {
        return unitOnTile;
    }

    public void Match() {
        transform.rotation = Quaternion.Euler(90f, 0, 0); // ???
        transform.position = new Vector3(coord.x, height, coord.y);
        transform.localScale = new Vector3(1, 1, 1);
        
    }

    public void Elevate() {
        height++;
        Match();
    }

    public void Lower() {
        height--;
        Match();
    }

    public void Load(Coord c, int h) {
        coord = c;
        height = h;
        Match();
    }

    public void Load(Vector3 v) {
        Load(new Coord((int) v.x, (int) v.z), (int) v.y);
    }
}
