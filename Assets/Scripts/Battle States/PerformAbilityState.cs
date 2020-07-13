using UnityEngine;
using System.Collections;
public class PerformAbilityState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        // TODO play animations, etc
        yield return null;
        // TODO apply ability effect, etc
        TemporaryAttackExample();
        print("past attack");

        if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else
        {
            print("to command select");
            owner.ChangeState<CommandSelectionState>();
        }
    }

    void TemporaryAttackExample()
    {
        for (int i = 0; i < turn.targets.Count; ++i)
        {
            Unit unit = turn.targets[i].GetUnitOnTile();
            Stats stats = unit != null ? unit.GetComponentInChildren<Stats>() : null;
            if (stats != null)
            {
                stats[StatTypes.HP] -= 50;
                if (stats[StatTypes.HP] <= 0)
                    stats[StatTypes.HP] = 0;
                units.Remove(unit);
                Destroy(unit.gameObject);
            }
        }
    }
}