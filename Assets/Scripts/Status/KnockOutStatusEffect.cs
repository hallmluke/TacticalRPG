using UnityEngine;
using System.Collections;
public class KnockOutStatusEffect : StatusEffect
{
  Unit owner;
  Stats stats;
  void Awake ()
  {
    owner = GetComponentInParent<Unit>();
    stats = owner.GetComponent<Stats>();
  }
  void OnEnable ()
  {
    owner.anim.SetBool("Dead", true);
    this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
    this.AddObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats); 
  }
  void OnDisable ()
  {
    owner.anim.SetBool("Dead", false);
    this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
    this.RemoveObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
  }
  void OnTurnCheck (object sender, object args)
  {
    // Dont allow a KO'd unit to take turns
    BaseException exc = args as BaseException;
    if (exc.defaultToggle == true)
      exc.FlipToggle();
  }
  void OnStatCounterWillChange (object sender, object args)
  {
    // Dont allow a KO'd unit to increment the turn order counter
    ValueChangeException exc = args as ValueChangeException;
    if (exc.toValue > exc.fromValue)
      exc.FlipToggle();
  }
}