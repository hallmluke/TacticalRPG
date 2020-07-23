using UnityEngine;
using System.Collections;
public class DefeatAllEnemiesVictoryCondition : BaseVictoryCondition 
{
  protected override void CheckForGameOver ()
  {
    base.CheckForGameOver();
    if (Victor == null && PartyDefeated(enemyTeam))
      Victor = playerTeam;
  }
}