using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState : State
{
    protected BattleManager owner;
   // public CameraRig cameraRig { get { return owner.cameraRig; } }
    public MouseCameraRig mouseCameraRig { get { return owner.mouseCameraRig; } }
    public Map map { get { return owner.map; } }
    public MapData mapData { get { return owner.mapData; } }
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
    public Coord pos { get { return owner.pos; } set { owner.pos = value; } }
    public ActionMenuController actionMenuController { get { return owner.actionMenuController; } }
    public UnitDisplayController unitDisplayController { get { return owner.unitDisplayController; } }
    public Turn turn { get { return owner.turn; } }
    public List<Unit> units { get { return owner.units; } }


    protected virtual void Awake()
    {
        owner = GetComponent<BattleManager>();
    }
    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
        InputController.mouseMoveEvent += OnMouse;
    }

    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
        InputController.mouseMoveEvent -= OnMouse;
    }
    protected virtual void OnMove(object sender, InfoEventArgs<Coord> e)
    {

    }

    protected virtual void OnFire(object sender, InfoEventArgs<int> e)
    {

    }

    protected virtual void OnMouse(object sender, InfoEventArgs<Vector3> e)
    {

    }
    protected virtual void SelectTile(Coord p)
    {
        if (pos == p || !map.tiles.ContainsKey(p))
            return;
        pos = p;
        tileSelectionIndicator.localPosition = map.tiles[p].center + new Vector3(0, .02f, 0);
    }

    protected virtual void SelectTile(Tile tile)
    {
        if (tile == null || tile.coord == pos)
        {
            return;
        }
        pos = tile.coord;
        tileSelectionIndicator.localPosition = tile.center + new Vector3(0, .02f, 0);
    }

    protected virtual void SelectTileFromMousePosition(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        Tile targetTile = null;

        if (Physics.Raycast(ray, out hit))
        {
            print("hit something?");
            targetTile = hit.transform.GetComponent<Tile>();

            if (targetTile == null && hit.transform.GetComponent<Unit>() != null)
            {

                targetTile = hit.transform.GetComponent<Unit>().currentTile;

            }
        }

        if (targetTile != null)
        {
            print(targetTile.coord);
        }

        SelectTile(targetTile);
    }

    protected virtual Unit GetUnit(Coord c)
    {
        Tile t = map.GetTileFromCoord(c);
        return t.GetUnitOnTile();
    }

    protected virtual void RefreshPrimaryStatPanel(Coord c)
    {
        Unit target = GetUnit(c);
        if (target != null)
            unitDisplayController.ShowPrimary(target.gameObject);
        else
            unitDisplayController.HidePrimary();
    }
    protected virtual void RefreshSecondaryStatPanel(Coord c)
    {
        Unit target = GetUnit(c);
        if (target != null)
            unitDisplayController.ShowSecondary(target.gameObject);
        else
            unitDisplayController.HideSecondary();
    }

}