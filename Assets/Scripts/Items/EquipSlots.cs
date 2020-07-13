using UnityEngine;
using System.Collections;
[System.Flags]
public enum EquipSlots
{
  None = 0,
  Weapon = 1 << 0,   
  Armor = 1 << 1,  
  Accessory = 1 << 2,

}