using UnityEngine;
using System.Collections;
public class EndFacingState : BattleState
{
    Directions startDir;
    public override void Enter()
    {
        base.Enter();
        startDir = turn.actor.dir;
        SelectTile(turn.actor.currentTile.coord);
    }

    protected override void OnMove(object sender, InfoEventArgs<Coord> e)
    {
        turn.actor.dir = e.info.GetDirection();
        turn.actor.SetWorldPositionFromMapPosition();
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        switch (e.info)
        {
            case 0:
                owner.ChangeState<SelectUnitState>();
                break;
            case 1:
                turn.actor.dir = startDir;
                turn.actor.SetWorldPositionFromMapPosition();
                owner.ChangeState<CommandSelectionState>();
                break;
        }
    }
}