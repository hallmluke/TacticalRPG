using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{

    public GameObject buttonPrefab;
    public Button attackButton;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject attackButtonObject = Instantiate(buttonPrefab) as GameObject;
        attackButtonObject.transform.SetParent(transform);
        
        attackButton = attackButtonObject.transform.GetComponent<Button>();
        attackButton.GetComponentInChildren<Text>().text = "Attack";
    }

    void Attack() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
