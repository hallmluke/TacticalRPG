using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Unit))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI() {
        

        Unit unit = target as Unit;

        if(DrawDefaultInspector()) {
            unit.SetWorldPositionFromMapPosition();
        }

        if(GUILayout.Button("Reset Position")) {
            unit.SetWorldPositionFromMapPosition();
        }
    }
}

