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
        if(turn.actor.anim != null) {
            Directions dir = turn.actor.currentTile.GetDirection(turn.targets[0]);
            Movement m = turn.actor.GetComponent<Movement>();
            yield return m.Turn(dir);
            turn.actor.anim.SetTrigger("Attack");
            yield return new WaitForSeconds(turn.actor.anim.GetCurrentAnimatorStateInfo(0).length);
        }
        
        yield return null;
        // TODO apply ability effect, etc
        ApplyAbility();

        if (IsBattleOver())
            owner.ChangeState<CutsceneState>();
        else if (!UnitHasControl())
            owner.ChangeState<SelectUnitState>();
        else if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else
            owner.ChangeState<CommandSelectionState>();

    }

    bool UnitHasControl()
    {
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }

    void ApplyAbility()
    {
        BaseAbilityEffect[] effects = turn.ability.GetComponentsInChildren<BaseAbilityEffect>();
        for (int i = 0; i < turn.targets.Count; ++i)
        {
            Tile target = turn.targets[i];
            for (int j = 0; j < effects.Length; ++j)
            {
                BaseAbilityEffect effect = effects[j];
                AbilityEffectTarget targeter = effect.GetComponent<AbilityEffectTarget>();
                if (targeter.IsTarget(target))
                {
                    HitRate rate = effect.GetComponent<HitRate>();
                    int chance = rate.Calculate(target);
                    if (UnityEngine.Random.Range(0, 101) > chance)
                    {
                        // A Miss!
                        continue;
                    }
                    effect.Apply(target);

                        
                }
            }
        }
    }
}