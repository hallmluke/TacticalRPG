using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor
{
    public MapGenerator map {
        get
        {
            return (MapGenerator) target;
        }
    }
    public override void OnInspectorGUI() {

        /*if(DrawDefaultInspector()) {
            map.GenerateMap();
        }*/

        if(GUILayout.Button("Generate Map")) {
            map.GenerateMap();
        }

        if(GUILayout.Button("Clear")) {
            map.Clear();
        }

        if(GUILayout.Button("Elevate")) {
            map.Elevate();
        }

        if(GUILayout.Button("Lower")) {
            map.Lower();
        }

        if(GUILayout.Button("Elevate Area")) {
            map.ElevateArea();
        }

        if(GUILayout.Button("Lower Area")) {
            map.LowerArea();
        }

        if(GUILayout.Button("Save")) {
            map.Save();
        }
        if(GUILayout.Button("Load")) {
            map.Load();
        }

        /*if(GUI.changed) {
            map.UpdateMarker();
        }*/
    }
}

