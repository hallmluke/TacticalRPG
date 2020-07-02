using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public override void Execute(Unit actor, Unit target) {
        target.ReceiveDamage(actor.totalAttack - target.totalDefense);
    }
}
