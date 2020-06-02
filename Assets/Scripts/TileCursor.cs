using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCursor : MonoBehaviour
{
    public const float HEIGHT = 0.75f;
    Map map;

    void Awake() {
        map = FindObjectOfType<Map>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPositionFromRaycast(RaycastHit hit) {
        if(hit.transform.GetComponent<Tile>() != null) {

                transform.position = new Vector3(hit.transform.position.x, HEIGHT, hit.transform.position.z);

            } else if (hit.transform.GetComponent<Unit>() != null) {

                Tile targetTile = hit.transform.GetComponent<Unit>().currentTile;

                transform.position = new Vector3(targetTile.transform.position.x, HEIGHT, targetTile.transform.position.z);
            }
    }
    /*
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit)) {
            if(hit.transform.GetComponent<Tile>() != null) {

                transform.position = new Vector3(hit.transform.position.x, HEIGHT, hit.transform.position.z);

            } else if (hit.transform.GetComponent<Unit>() != null) {

                Tile targetTile = hit.transform.GetComponent<Unit>().currentTile;

                transform.position = new Vector3(targetTile.transform.position.x, HEIGHT, targetTile.transform.position.z);
            }

        }
    }
    */
}
