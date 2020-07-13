using UnityEngine;
using System.Collections;
public class DefaultAbilityEffectTarget : AbilityEffectTarget 
{
  public override bool IsTarget (Tile tile) {
    if (tile == null || tile.GetUnitOnTile() == null) {
      return false;
    }
    Stats s = tile.GetUnitOnTile().GetComponent<Stats>();
    return s != null && s[StatTypes.HP] > 0;
  }
}