using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnitAbilityArea : AbilityArea 
{
  public override HashSet<Tile> GetTilesInArea (Map map, Coord coord) {
    HashSet<Tile> retValue = new HashSet<Tile>();
    Tile tile = map.GetTileFromCoord(coord);
    if (tile != null)
      retValue.Add(tile);
    return retValue;
  }
}