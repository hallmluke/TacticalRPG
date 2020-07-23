using UnityEngine;
using System.Collections;
public abstract class BaseVictoryCondition : MonoBehaviour
{
    public Team Victor
    {
        get { return victor; }
        protected set { victor = value; }
    }
    Team victor = null;
    public Team playerTeam;
    public Team enemyTeam;

    protected BattleManager bm;
    protected virtual void Awake()
    {
        bm = GetComponent<BattleManager>();
        if(!playerTeam) {
            playerTeam = bm.playerTeam;
        }
        if(!enemyTeam) {
            enemyTeam = bm.enemyTeam;
        }
    }

    protected virtual void OnEnable()
    {
        this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }
    protected virtual void OnDisable()
    {
        this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }
    protected virtual void OnHPDidChangeNotification(object sender, object args)
    {
        CheckForGameOver();
    }

    protected virtual bool IsDefeated(Unit unit)
    {
        Health health = unit.GetComponent<Health>();
        if (health)
            return health.MinHP == health.HP;

        Stats stats = unit.GetComponent<Stats>();
        return stats[StatTypes.HP] == 0;
    }

    protected virtual bool PartyDefeated(Team party)
    {
        for (int i = 0; i < bm.units.Count; ++i)
        {
            Team a = bm.units[i].team;
            if (a == null)
                continue;
            if (a == party && !IsDefeated(bm.units[i]))
                return false;
        }
        return true;
    }

    protected virtual void CheckForGameOver()
    {


        if (PartyDefeated(playerTeam)) {
            Victor = enemyTeam;
        }
    }

    // ... Add next code samples here
}