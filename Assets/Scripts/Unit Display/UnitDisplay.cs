using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDisplay : MonoBehaviour
{
    public LayoutAnchorPanel panel;
    public Image background;
    public Image avatar;
    public Text nameLabel;
    public Text hpLabel;
    public Text mpLabel;
    public Text lvLabel;
    public void Display (GameObject obj) {
    // Temp until I add a component to determine unit alliances
    Unit unit = obj.GetComponent<Unit>();
    if(unit == null) {
      print("unit null");
    }
    if(unit.team == null) {
      print("no team");
    }

    background.sprite = obj.GetComponent<Unit>().team.unitDisplayBackground;
    // avatar.sprite = null; TODO Need a component which provides this data
    nameLabel.text = obj.name;
    Stats stats = obj.GetComponent<Stats>();
    if (stats)
    {
      hpLabel.text = string.Format( "HP {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
      mpLabel.text = string.Format( "MP {0} / {1}", stats[StatTypes.MP], stats[StatTypes.MMP] );
      lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LVL]);
    }
  }
}
