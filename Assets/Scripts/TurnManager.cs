using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{

    public ActionMenu actionMenu;
    public UnitDisplay unitDisplay;

    public GameFlowManager gameFlowManager;

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

    Tile originalUnitPosition;
    HashSet<Tile> movableTiles;
    HashSet<Tile> targetableTiles;

    List<Tile> movementPath;
    Map map;
    Ray ray;
    RaycastHit hit;

    public bool active = true;



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
        if(active) {
        switch(currentState) {
            case TurnState.Unselected:
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (tileCursor != null)
                    {
                        tileCursor.SetPositionFromRaycast(hit);
                    }

                    UpdateUnitDisplay();

                    if (Input.GetMouseButtonDown(0))
                    {
                        if(SelectUnit()) {
                            originalUnitPosition = selectedUnit.currentTile;
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
                    Tile targetTile = hit.transform.GetComponent<Tile>();

                    if(targetTile == null && hit.transform.GetComponent<Unit>() != null) {

                        targetTile = hit.transform.GetComponent<Unit>().currentTile;
                        
                    }

                    if (tileCursor != null)
                    {
                        tileCursor.SetPositionFromRaycast(hit);
                    }

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
                }
                break;
            case TurnState.Moved:
                if(!actionMenu.gameObject.activeSelf) {
                    actionMenu.gameObject.SetActive(true);
                    
                    actionMenu.RegisterListener(actionMenu.attackButton, OpenAttackTargets);
                    actionMenu.RegisterListener(actionMenu.waitButton, Wait);
                    actionMenu.RegisterListener(actionMenu.cancelButton, CancelMove);
                    
                }
                // TODO: Menu UI
                break;
            case TurnState.Targeting:
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    UpdateUnitDisplay();

                    if (tileCursor != null)
                    {
                        tileCursor.SetPositionFromRaycast(hit);
                    }

                    Tile targetTile = hit.transform.GetComponent<Tile>();

                    if(targetTile == null && hit.transform.GetComponent<Unit>() != null) {

                        targetTile = hit.transform.GetComponent<Unit>().currentTile;

                    }

                    if (targetTile != activeTile && targetTile != null)
                    {
                        
                        activeTile = targetTile;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if(targetableTiles.Contains(targetTile)) {
                            AttackUnit();
                            currentState = TurnState.Acted;
                        }
                    }
                }
                // TODO: Actions
                break;
            case TurnState.Acted:
                selectedUnit.EndTurn();
                gameFlowManager.CheckForEndGame();
                gameFlowManager.CheckForAllUnitsMoved();
                selectedUnit = null;
                currentState = TurnState.Unselected;
                // TODO: Menu UI
                break;
            default:
                print("An unexpected turn state in TurnManager occurred");
                break;
        }
        }
        
    }

    void UpdateUnitDisplay() {
        if (hit.transform.GetComponent<Tile>() != null)
        {
            unitDisplay.UpdateDisplay(hit.transform.GetComponent<Tile>().GetUnitOnTile());
        }
        else if (hit.transform.GetComponent<Unit>() != null)
        {
            unitDisplay.UpdateDisplay(hit.transform.GetComponent<Unit>());

        } else {

            unitDisplay.UpdateDisplay(null);
        }

    }
    void OpenAttackTargets() {
        targetableTiles = selectedUnit.FindAttackTargets();

        if(targetableTiles.Count > 0) {
            currentState = TurnState.Targeting;
        } else {
            print("No targets");

            currentState = TurnState.Acted;
        }
        actionMenu.CloseMenu();
        
    }

    void Wait() {
        currentState = TurnState.Acted;
        actionMenu.CloseMenu();
    }

    void CancelMove() {

        selectedUnit.CancelMove(originalUnitPosition);

        originalUnitPosition = null;

        currentState = TurnState.Unselected;

        actionMenu.CloseMenu();

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

            if(gameFlowManager.currentTeam == selectedUnit.team && !selectedUnit.acted) {
                return true;
            } else {
                selectedUnit = null;
                return false;
            }
            
        }
        else if (hit.transform.GetComponent<Tile>() != null)
        {
            Tile selectedTile = hit.transform.GetComponent<Tile>();

            Unit unitOnTile = selectedTile.GetUnitOnTile();

            if (unitOnTile != null)
            {
                if(gameFlowManager.currentTeam == unitOnTile.team && !unitOnTile.acted) {
                    selectedUnit = unitOnTile;
                    return true;
                } else {
                    selectedUnit = null;
                    return false;
                }
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

    void AttackUnit() {

        selectedUnit.ExecuteAction(activeTile, new Attack());

        foreach(Tile tile in targetableTiles) {
            tile.isAttackable = false;
            tile.parent = null;
        }

        targetableTiles = null;
    }

}
