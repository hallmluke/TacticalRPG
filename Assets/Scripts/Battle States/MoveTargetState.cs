using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveTargetState : BattleState
{
    HashSet<Tile> tiles;

    public override void Enter()
    {
        base.Enter();
        Movement mover = owner.turn.actor.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(map);
        map.SelectTiles(tiles);
        RefreshPrimaryStatPanel(pos);
    }

    public override void Exit()
    {
        base.Exit();
        map.DeSelectTiles(tiles);
        tiles = null;
        unitDisplayController.HidePrimary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Coord> e)
    {
        SelectTile(e.info + pos);
        RefreshPrimaryStatPanel(pos);
    }

    protected override void OnMouse(object sender, InfoEventArgs<Vector3> e)
    {
        SelectTileFromMousePosition(e.info);
        RefreshPrimaryStatPanel(pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
                owner.ChangeState<MoveSequenceState>();
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
}