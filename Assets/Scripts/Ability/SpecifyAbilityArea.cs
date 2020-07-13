using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpecifyAbilityArea : AbilityArea 
{
  public int horizontal;
  public int vertical;
  Tile tile;
  public override HashSet<Tile> GetTilesInArea (Map map, Coord coord) {
    tile = map.GetTileFromCoord(coord);
    return map.Search(tile, ExpandSearch);
  }
  bool ExpandSearch (Tile from, Tile to) {
    return (from.distance + 1) <= horizontal && Mathf.Abs(to.height - tile.height) <= vertical;
  }
}