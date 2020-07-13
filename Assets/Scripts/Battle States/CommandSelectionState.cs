using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CommandSelectionState : BaseActionMenuState
{

    public override void Enter()
    {
        base.Enter();
        unitDisplayController.ShowPrimary(turn.actor.gameObject);
    }
    public override void Exit()
    {
        base.Exit();
        unitDisplayController.HidePrimary();
    }
    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Commands";
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Action");
            menuOptions.Add("Wait");
        }
        actionMenuController.Show(menuTitle, menuOptions);
        actionMenuController.SetLocked(0, turn.hasUnitMoved);
        actionMenuController.SetLocked(1, turn.hasUnitActed);
    }

    protected override void Confirm()
    {
        switch (actionMenuController.selection)
        {
            case 0: // Move
                owner.ChangeState<MoveTargetState>();
                break;
            case 1: // Action
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2: // Wait
                owner.ChangeState<EndFacingState>();
                break;
        }
    }

    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            actionMenuController.SetLocked(0, false);
            SelectTile(turn.actor.currentTile.coord);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }
}
