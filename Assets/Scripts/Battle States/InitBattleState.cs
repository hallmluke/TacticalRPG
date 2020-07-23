using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        map.Load(mapData);
        Coord p = new Coord((int)mapData.tiles[0].x, (int)mapData.tiles[0].z);
        SelectTile(p);
        yield return null;
        SpawnTestUnits();
        AddVictoryCondition();
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        owner.ChangeState<SelectUnitState>();
    }

    void SpawnTestUnits()
    {
        //   System.Type[] components = new System.Type[] { typeof(WalkMovement), typeof(FlyMovement), typeof(TeleportMovement) };
        string[] recipes = new string[]
  {
    "Player1",
    "Player1",
    "Enemy1",
    "Enemy1"
  };
        List<Tile> locations = new List<Tile>(map.tiles.Values);
        for (int i = 0; i < recipes.Length; ++i)
        {
            int level = UnityEngine.Random.Range(9, 12);
            print(playerTeam.teamName);
            Team targetTeam;
            if(i < 2) {
                targetTeam = playerTeam;
            } else {
                targetTeam = enemyTeam;
            }
            print(targetTeam.teamName);
            GameObject instance = UnitFactory.Create(recipes[i], level);
            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
            unit.SetWorldPositionFromMapPosition();
            units.Add(unit);
            unit.team = targetTeam;
            
        }
        SelectTile(units[0].currentTile.coord);
    }

    void AddVictoryCondition()
    {
        DefeatAllEnemiesVictoryCondition vc = owner.gameObject.AddComponent<DefeatAllEnemiesVictoryCondition>();

    }
}