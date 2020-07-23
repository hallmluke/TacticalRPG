using UnityEngine;
using System.Collections;
public class SelectUnitState : BattleState 
{
  public override void Enter ()
  {
    base.Enter ();
    StartCoroutine("ChangeCurrentUnit");
  }
  public override void Exit ()
  {
    base.Exit ();
    unitDisplayController.HidePrimary();
  }
  IEnumerator ChangeCurrentUnit ()
  {
    owner.round.MoveNext();
    SelectTile(turn.actor.currentTile.coord);
    RefreshPrimaryStatPanel(pos);
    yield return null;
    owner.ChangeState<CommandSelectionState>();
  }
}