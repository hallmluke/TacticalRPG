using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDisplay : MonoBehaviour
{
    public Unit displayedUnit;
    public TMP_Text unitName;
    public TMP_Text health;
    public TMP_Text job;

    bool enabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void UpdateDisplay(Unit unitToDisplay) {
        displayedUnit = unitToDisplay;
        if(displayedUnit != null && enabled) {
            gameObject.SetActive(true);
            unitName.text = displayedUnit.displayName;
            health.text = displayedUnit.currentHealth + "/" + displayedUnit.maxHealth;
            job.text = displayedUnit.job.jobName;
        } else {
            gameObject.SetActive(false);
        }
    }
}
