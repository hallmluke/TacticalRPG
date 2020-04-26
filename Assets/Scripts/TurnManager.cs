using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Unit selectedUnit;
    HashSet<Coord> movableTiles;
    Map map;

    void Awake() {
        map = FindObjectOfType<Map>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedUnit != null) {

            if(movableTiles == null) {
                movableTiles = selectedUnit.FindMovementOptions();
            }
            if(Input.GetMouseButtonDown(0)) {
                MoveUnit();
            }

        } else {

            if(Input.GetMouseButtonDown(0)) {
                SelectUnit();
            }
            
        }
    }

    void SelectUnit() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit)) {
            if(hit.transform.GetComponent<Unit>() != null) {

                selectedUnit = hit.transform.GetComponent<Unit>();

            } else if(hit.transform.GetComponent<Tile>() != null) {

                print("Clicked on Tile");

                Tile selectedTile = hit.transform.GetComponent<Tile>();

                if(selectedTile.GetUnitOnTile() != null) {

                    selectedUnit = selectedTile.GetUnitOnTile();

                }

            }
        }

        print(selectedUnit);
    }

    void MoveUnit() {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit)) {

            if(hit.transform.GetComponent<Tile>() != null) {

                Tile selectedTile = hit.transform.GetComponent<Tile>();

                if(movableTiles.Contains(selectedTile.coord)) {

                    selectedUnit.Move(selectedTile.coord);
                    selectedUnit = null;
                    foreach(Coord movable in movableTiles) {
                        Tile tile = map.tileMap[movable.x, movable.y].GetComponent<Tile>();
            
                        tile.isMovable = false;
                    }

                    movableTiles = null;

                }

            }

        }
    }

}
