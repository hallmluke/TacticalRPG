using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ConstantAbilityRange : AbilityRange 
{
  public override HashSet<Tile> GetTilesInRange (Map map) {
    return map.Search(unit.currentTile, ExpandSearch);
  }
  bool ExpandSearch (Tile from, Tile to) {
    return (from.distance + 1) <= horizontal && Mathf.Abs(to.height - unit.currentTile.height) <= vertical;
  }
}