using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action //: MonoBehaviour
{
    public abstract void Execute(Unit actor, Unit target);
}
