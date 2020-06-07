using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDisplay : MonoBehaviour
{
    public Unit displayedUnit;
    public TMP_Text unitName;
    public TMP_Text health;

    bool enabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(displayedUnit != null && enabled) {
            gameObject.SetActive(true);
            unitName.text = displayedUnit.displayName;
            health.text = displayedUnit.currentHealth + "/" + displayedUnit.maxHealth;
        } else {
            gameObject.SetActive(false);
        }
    }
}
