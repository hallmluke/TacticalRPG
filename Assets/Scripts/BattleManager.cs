using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : StateMachine 
{
//  public CameraRig cameraRig;
  public MouseCameraRig mouseCameraRig;
  public Map map;
  public MapData mapData;
  public Transform tileSelectionIndicator;
  public Coord pos;

  public GameObject playerPrefab;
  public Tile currentTile { get { return map.GetTileFromCoord(pos); }}

  public ActionMenuController actionMenuController;
  public UnitDisplayController unitDisplayController;
  public Turn turn = new Turn();
  public List<Unit> units = new List<Unit>();


  void Start ()
  {
    ChangeState<InitBattleState>();
  }
}