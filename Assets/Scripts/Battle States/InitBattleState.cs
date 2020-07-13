using UnityEngine;
using System.Collections;
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
        owner.ChangeState<SelectUnitState>();
    }

    void SpawnTestUnits()
    {
        //   System.Type[] components = new System.Type[] { typeof(WalkMovement), typeof(FlyMovement), typeof(TeleportMovement) };
        for (int i = 0; i < 3; ++i)
        {
            GameObject instance = Instantiate(owner.playerPrefab) as GameObject;

            Stats s = instance.AddComponent<Stats>();
            s[StatTypes.LVL] = 1;
            GameObject jobPrefab = Resources.Load<GameObject>("Jobs/Soldier");
            GameObject jobInstance = Instantiate(jobPrefab) as GameObject;

            jobInstance.transform.SetParent(instance.transform);
            Job job = jobInstance.GetComponent<Job>();
            job.Employ();
            job.LoadDefaultStats();

            Coord p = new Coord((int)mapData.tiles[i].x, (int)mapData.tiles[i].z);
            print(p.ToString());
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(map.GetTileFromCoord(p));

            unit.SetWorldPositionFromMapPosition();
            instance.AddComponent<WalkMovement>();

            units.Add(unit);
        }
    }
}