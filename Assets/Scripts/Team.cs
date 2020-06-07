﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public string teamName;
    public int teamNumber;
    public List<Unit> unitsInTeam;
    HashSet<Unit> unitsLeftToMove;

    public void ResetUnitMoveState() {
        unitsLeftToMove.Clear();
        foreach(Unit unit in unitsInTeam) {
            unitsLeftToMove.Add(unit);
        }
    }

    public bool AllUnitsMoved() {
        return unitsLeftToMove.Count == 0;
    }

    public void RemoveUnitFromLeftToMove(Unit unit) {
        unitsLeftToMove.Remove(unit);
    }
    // Start is called before the first frame update
    void Start()
    {
        unitsLeftToMove = new HashSet<Unit>();
        ResetUnitMoveState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
