using UnityEngine;
using System.Collections;
public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
        mouseCameraRig.lockPosition = turn.actor.transform;
        mouseCameraRig.lockToPos = true;
    }

    public override void Exit()
    {
        base.Exit();
        mouseCameraRig.lockPosition = mouseCameraRig.cursorPosition;
    }

    IEnumerator Sequence()
    {
        Movement m = turn.actor.GetComponent<Movement>();
        if(turn.actor.anim != null) {
            turn.actor.anim.SetBool("Walk", true);
        }
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        turn.hasUnitMoved = true;
        if(turn.actor.anim != null) {
            turn.actor.anim.SetBool("Walk", false);
        }
        owner.ChangeState<CommandSelectionState>();
    }
}