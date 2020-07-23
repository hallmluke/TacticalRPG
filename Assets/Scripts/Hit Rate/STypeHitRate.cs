using UnityEngine;
using System.Collections;
public class STypeHitRate : HitRate 
{
  public override int Calculate (Tile target)
  {
    Unit defender = target.GetUnitOnTile();
    if (AutomaticMiss(defender))
      return Final(100);
    if (AutomaticHit(defender))
      return Final(0);
    int res = GetResistance(defender);
    res = AdjustForStatusEffects(defender, res);
    res = AdjustForRelativeFacing(attacker, defender, res);
    res = Mathf.Clamp(res, 0, 100);
    return Final(res);
  }
  int GetResistance (Unit target)
  {
    Stats s = target.GetComponentInParent<Stats>();
    return s[StatTypes.RES];
  }
  int AdjustForRelativeFacing (Unit attacker, Unit target, int rate)
  {
    switch (attacker.GetFacing(target))
    {
    case Facings.Front:
      return rate;
    case Facings.Side:
      return rate - 10;
    default:
      return rate - 20;
    }
  }
}