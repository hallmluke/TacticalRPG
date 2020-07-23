using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CategorySelectionState : BaseActionMenuState
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
            menuOptions = new List<string>();
        else
            menuOptions.Clear();
        menuTitle = "Action";
        menuOptions.Add("Attack");
        AbilityCatalog catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
        for (int i = 0; i < catalog.CategoryCount(); ++i)
            menuOptions.Add(catalog.GetCategory(i).name);

        actionMenuController.Show(menuTitle, menuOptions);
    }
    protected override void Confirm()
    {
        if (actionMenuController.selection == 0)
            Attack();
        else
            SetCategory(actionMenuController.selection - 1);
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }
    void Attack()
    {
        turn.ability = turn.actor.GetComponentInChildren<Ability>();
        owner.ChangeState<AbilityTargetState>();
    }
    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}