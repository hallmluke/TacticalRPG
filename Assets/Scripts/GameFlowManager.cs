using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    public TurnManager turnManager;

    public List<Team> teams;
    public Team currentTeam;

    public TMP_Text teamDisplay;
    public int currentTeamNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentTeam = teams[currentTeamNumber];
        UpdateTeamDisplay();
        UpdateUnitsOnNewTurn();
    }

    void UpdateUnitsOnNewTurn() {
        foreach(Unit unit in currentTeam.unitsInTeam) {
            unit.NewTurn();
        }
    }
    void SwitchCurrentTeam() {
        currentTeamNumber = (currentTeamNumber + 1) % teams.Count;
        currentTeam = teams[currentTeamNumber];
    }

    void UpdateTeamDisplay() {
        teamDisplay.text = currentTeam.teamName;
    }

    void NextTurn() {
        SwitchCurrentTeam();
        UpdateTeamDisplay();
        UpdateUnitsOnNewTurn();
    }

    public void CheckForAllUnitsMoved() {
        if(currentTeam.AllUnitsMoved()) {
            NextTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
