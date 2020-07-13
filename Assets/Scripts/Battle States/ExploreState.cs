using UnityEngine;
using System.Collections;
public class ExploreState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        RefreshPrimaryStatPanel(pos);
    }

    public override void Exit()
    {
        base.Exit();
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
            owner.ChangeState<CommandSelectionState>();
    }
}