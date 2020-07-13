using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ActionSelectionState : BaseActionMenuState
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
    public static int category;
    string[] whiteMagicOptions = new string[] { "Cure", "Raise", "Holy" };
    string[] blackMagicOptions = new string[] { "Fire", "Ice", "Lightning" };
    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>(3);
        if (category == 0)
        {
            menuTitle = "White Magic";
            SetOptions(whiteMagicOptions);
        }
        else
        {
            menuTitle = "Black Magic";
            SetOptions(blackMagicOptions);
        }
        actionMenuController.Show(menuTitle, menuOptions);
    }
    protected override void Confirm()
    {
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        owner.ChangeState<CommandSelectionState>();
    }
    protected override void Cancel()
    {
        owner.ChangeState<CategorySelectionState>();
    }
    void SetOptions(string[] options)
    {
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }
}