using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public ActionMenu actionMenu;

    public enum TurnState {
        Unselected, // No unit selected
        SelectedInactive, // Selected unit, but not one whose turn it is to move
        Selected, // Selected unit
        SelectingMove,
        Moving, // Unit is currently in the process of moving
        Moved, // Unit has finished moving
        Targeting, // Unit is targeting another unit with an action
        Acted // Unit has finished turn
    }

    TurnState currentState = TurnState.Unselected;

    public TileCursor tileCursor;

    Unit selectedUnit;
    Tile activeTile;

    HashSet<Tile> movableTiles;
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
        switch(currentState) {
            case TurnState.Unselected:
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (tileCursor != null)
                    {
                        tileCursor.SetPositionFromRaycast(hit);
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if(SelectUnit()) {
                            currentState = TurnState.SelectingMove;
                        }
                    }
                }
                break;
            case TurnState.SelectedInactive:
                // TODO: Menu UI
                break;
            case TurnState.Selected:
                // TODO: Menu UI
                break;
            case TurnState.SelectingMove:
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (tileCursor != null)
                    {
                        tileCursor.SetPositionFromRaycast(hit);
                    }

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
                        if(movableTiles.Contains(activeTile)) {
                            MoveUnit();
                            currentState = TurnState.Moving;
                        }
                    }
                }
                break;
            case TurnState.Moving:
                if(!selectedUnit.moving) {
                    currentState = TurnState.Moved;
                    selectedUnit = null;
                }
                break;
            case TurnState.Moved:
                if(!actionMenu.gameObject.activeSelf) {
                    actionMenu.gameObject.SetActive(true);
                    
                    actionMenu.attackButton.onClick.AddListener(OpenAttackTargets);
                    
                }
                // TODO: Menu UI
                break;
            case TurnState.Targeting:
                // TODO: Actions
                break;
            case TurnState.Acted:
                // TODO: Menu UI
                break;
            default:
                print("An unexpected turn state in TurnManager occurred");
                break;
        }
        
    }

    void OpenAttackTargets() {
        print("Attack button clicked");
        currentState = TurnState.Unselected;
        actionMenu.gameObject.SetActive(false);
        actionMenu.attackButton.onClick.RemoveListener(OpenAttackTargets);
    }

    void CalculatePath()
    {
        ResetPath();

        Tile targetTile = hit.transform.GetComponent<Tile>();
        Tile currentTile = targetTile;

        while (currentTile != null)
        {
            movementPath.Add(currentTile);
            currentTile.inPath = true;
            currentTile = currentTile.parent;
        }
    }

    void ResetPath() {
        while (movementPath.Count > 0)
        {
            Tile oldPathTile = movementPath[movementPath.Count - 1];
            movementPath.Remove(oldPathTile);
            oldPathTile.inPath = false;
        }
    }

    bool SelectUnit()
    {

        if (hit.transform.GetComponent<Unit>() != null)
        {
            selectedUnit = hit.transform.GetComponent<Unit>();
            return true;
        }
        
        if (hit.transform.GetComponent<Tile>() != null)
        {
            Tile selectedTile = hit.transform.GetComponent<Tile>();

            if (selectedTile.GetUnitOnTile() != null)
            {
                selectedUnit = selectedTile.GetUnitOnTile();
                return true;
            }

        }

        return false;
    }

    void MoveUnit()
    {
            selectedUnit.Move(activeTile.coord, movementPath);

            ResetPath();

            foreach (Tile tile in movableTiles)
            {

                tile.isMovable = false;
                tile.parent = null;

            }
            movableTiles = null;

    }

}
