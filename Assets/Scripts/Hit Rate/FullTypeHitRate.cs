using UnityEngine;
using System.Collections;
public class FullTypeHitRate : HitRate 
{
  public override int Calculate (Tile target)
  {
    if (AutomaticMiss(target.GetUnitOnTile()))
      return Final(100);
    return Final (0);
  }
}