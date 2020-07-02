using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour
{

    public string jobName;
    public string description;

    // Stat Modifiers
    public float healthModifier;
    public float attackModifier;
    public float defenseModifier;
    public float spAttackModifier;
    public float spDefenseModifier;
    public float speedModifier;

    // Stat Growth Modifiers

    public float healtGrowthhModifier;
    public float attackGrowthModifier;
    public float defenseGrowthModifier;
    public float spAttackGrowthModifier;
    public float spDefenseGrowthModifier;
    public float speedGrowthModifier;

    // Abilities
    public List<Ability> abilities;

}
