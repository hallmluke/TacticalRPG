using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbilityTargetState : BattleState
{
    HashSet<Tile> tiles;
    AbilityRange ar;
    public override void Enter()
    {
        base.Enter();
        ar = turn.ability.GetComponent<AbilityRange>();
        SelectTiles();
        unitDisplayController.ShowPrimary(turn.actor.gameObject);
        if (ar.directionOriented)
            RefreshSecondaryStatPanel(pos);
    }
    public override void Exit()
    {
        base.Exit();
        map.DeSelectTiles(tiles);
        unitDisplayController.HidePrimary();
        unitDisplayController.HideSecondary();
    }
    protected override void OnMove(object sender, InfoEventArgs<Coord> e)
    {
        if (ar.directionOriented)
        {
            ChangeDirection(e.info);
        }
        else
        {
            SelectTile(e.info + pos);
            RefreshSecondaryStatPanel(pos);
        }
    }

    protected override void OnMouse(object sender, InfoEventArgs<Vector3> e)
    {
        SelectTileFromMousePosition(e.info);

        RefreshSecondaryStatPanel(pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (ar.directionOriented || tiles.Contains(map.GetTileFromCoord(pos)))
            {
                print("to confirm ability target state");
                owner.ChangeState<ConfirmAbilityTargetState>();
            }
        }
        else
        {
            owner.ChangeState<CategorySelectionState>();
        }
    }
    void ChangeDirection(Coord c)
    {
        Directions dir = c.GetDirection();
        if (turn.actor.dir != dir)
        {
            map.DeSelectTiles(tiles);
            turn.actor.dir = dir;
            turn.actor.SetWorldPositionFromMapPosition();
            SelectTiles();
        }
    }
    void SelectTiles()
    {
        tiles = ar.GetTilesInRange(map);
        map.SelectTiles(tiles);
    }
}