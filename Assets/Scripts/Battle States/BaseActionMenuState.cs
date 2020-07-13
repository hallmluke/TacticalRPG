using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class BaseActionMenuState : BattleState
{
    protected string menuTitle;
    protected List<string> menuOptions;
    public override void Enter()
    {
        base.Enter();
        SelectTile(turn.actor.currentTile.coord);
        mouseCameraRig.lockToPos = true;
        LoadMenu();
    }
    public override void Exit()
    {
        base.Exit();
        actionMenuController.Hide();
        mouseCameraRig.lockToPos = false;
    }
    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            Confirm();
        else
            Cancel();
    }
    protected override void OnMove(object sender, InfoEventArgs<Coord> e)
    {
        if (e.info.x > 0 || e.info.y < 0)
            actionMenuController.Next();
        else
            actionMenuController.Previous();
    }
    protected abstract void LoadMenu();
    protected abstract void Confirm();
    protected abstract void Cancel();
}