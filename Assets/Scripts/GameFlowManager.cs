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

    public Vector3 cam_position;


    // Start is called before the first frame update
    void Start()
    {
        currentTeam = teams[currentTeamNumber];
        UpdateTeamDisplay();
        UpdateUnitsOnNewTurn();
        cam_position = Camera.main.transform.position;
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
        cam_position = Camera.main.transform.position;

        currentTeam.ResetUnitMoveState();
        UpdateTeamDisplay();
        UpdateUnitsOnNewTurn();
    }

    public void CheckForAllUnitsMoved() {
        if(currentTeam.AllUnitsMoved()) {
            NextTurn();
        }
    }

    public void CheckForEndGame() {
        foreach(Team team in teams) {
            if(!team.IsTeamAlive()) {
                print(team.teamName + " defeated.");
                turnManager.active = false;
            }
        }
    }

}
