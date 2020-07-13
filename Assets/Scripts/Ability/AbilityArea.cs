using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class AbilityArea : MonoBehaviour
{
  public abstract HashSet<Tile> GetTilesInArea (Map map, Coord coord);
}