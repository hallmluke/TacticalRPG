using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI() {
        

        Map map = target as Map;

        if(DrawDefaultInspector()) {
            map.GenerateMap();
        }

        if(GUILayout.Button("Generate Map")) {
            map.GenerateMap();
        }
    }
}

