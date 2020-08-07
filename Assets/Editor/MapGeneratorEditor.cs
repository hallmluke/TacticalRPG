using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public MapGenerator map {
        get
        {
            return (MapGenerator) target;
        }
    }

    public enum Modes {
        Select,
        Raise,
        Lower
    }
    public Modes currentMode;
    public MapData mapData;

    void OnSceneGUI() {
        int ControlID = GUIUtility.GetControlID(FocusType.Passive);

        if(Event.current.type == EventType.MouseDown) {
            RaycastHit hit;
            if(Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit)) {
                Tile targetTile = hit.transform.GetComponent<Tile>();
                
                if(targetTile != null) {
                    if(currentMode == Modes.Raise) {
                        map.ElevateSingle(targetTile.coord);
                    } else if (currentMode == Modes.Lower) {
                        map.LowerSingle(targetTile.coord);
                    }
                }
            }
        }
        if(Event.current.type == EventType.Layout) {
            HandleUtility.AddDefaultControl(ControlID);
        }
    }
    public override void OnInspectorGUI() {

        DrawDefaultInspector();

        var text = new string[] {"Select", "Raise", "Lower"};

        currentMode = (Modes) GUILayout.SelectionGrid((int) currentMode, text, 1, EditorStyles.radioButton);
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

