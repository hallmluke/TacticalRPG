using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public TileCursor tileCursor;

    Unit selectedUnit;
    Tile activeTile;

    HashSet<Coord> movableTiles;
    List<Tile> movementPath;
    Map map;
    Ray ray;
    RaycastHit hit;



    void Awake()
    {
        map = FindObjectOfType<Map>();
        movementPath = new List<Tile>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (tileCursor != null)
            {
                tileCursor.SetPositionFromRaycast(hit);
            }

            if (selectedUnit != null)
            {
                Tile targetTile = hit.transform.GetComponent<Tile>();

                if (movableTiles == null)
                {
                    movableTiles = selectedUnit.FindMovementOptions();
                }
                if (targetTile != activeTile && targetTile != null)
                {
                    activeTile = targetTile;
                    CalculatePath();
                }
                if (Input.GetMouseButtonDown(0))
                {

                    MoveUnit();
                }

            }
            else
            {

                if (Input.GetMouseButtonDown(0))
                {
                    SelectUnit();
                }

            }
        }
    }

    void CalculatePath()
    {

        while (movementPath.Count > 0)
        {
            Tile oldPathTile = movementPath[movementPath.Count - 1];
            movementPath.Remove(oldPathTile);
            print($"Old Path: X: {oldPathTile.coord.x}, Y: {oldPathTile.coord.y}");
            oldPathTile.inPath = false;
        }

        Tile targetTile = hit.transform.GetComponent<Tile>();
        Tile currentTile = targetTile;

        while (currentTile != null)
        {
            movementPath.Add(currentTile);
            currentTile.inPath = true;
            currentTile = currentTile.parent;
        }
    }

    void SelectUnit()
    {

        if (hit.transform.GetComponent<Unit>() != null)
        {

            selectedUnit = hit.transform.GetComponent<Unit>();

        }
        else if (hit.transform.GetComponent<Tile>() != null)
        {

            print("Clicked on Tile");

            Tile selectedTile = hit.transform.GetComponent<Tile>();

            if (selectedTile.GetUnitOnTile() != null)
            {

                selectedUnit = selectedTile.GetUnitOnTile();

            }

        }

        print(selectedUnit);
    }

    void MoveUnit()
    {

        if (movableTiles.Contains(activeTile.coord))
        {

            selectedUnit.Move(activeTile.coord, movementPath);
            selectedUnit = null;
            foreach (Coord movable in movableTiles)
            {
                Tile tile = map.tileMap[movable.x, movable.y].GetComponent<Tile>();

                tile.isMovable = false;
                tile.inPath = false;
                tile.parent = null;
            }

            movableTiles = null;

        }

    }

}
