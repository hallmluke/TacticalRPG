using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionMenu : MonoBehaviour
{

    public GameObject buttonPrefab;
    public Button attackButton;
    public Button waitButton;
    public Button cancelButton;

    Dictionary<Button, UnityAction> listeners;
    // Start is called before the first frame update
    void Awake()
    {
        listeners = new Dictionary<Button, UnityAction>();

        // TODO: Not hardcode buttons, fill menu dynamically from available actions
        GameObject attackButtonObject = Instantiate(buttonPrefab) as GameObject;
        attackButtonObject.transform.SetParent(transform);
        
        attackButton = attackButtonObject.transform.GetComponent<Button>();
        attackButton.GetComponentInChildren<Text>().text = "Attack";

        GameObject waitButtonObject = Instantiate(buttonPrefab) as GameObject;
        waitButtonObject.transform.SetParent(transform);
        
        waitButton = waitButtonObject.transform.GetComponent<Button>();
        waitButton.GetComponentInChildren<Text>().text = "Wait";

        GameObject cancelButtonObject = Instantiate(buttonPrefab) as GameObject;
        cancelButtonObject.transform.SetParent(transform);
        
        cancelButton = cancelButtonObject.transform.GetComponent<Button>();
        cancelButton.GetComponentInChildren<Text>().text = "Cancel";
    }

    public void RegisterListener(Button button, UnityAction callback) {
        button.onClick.AddListener(callback);
        listeners.Add(button, callback);
    }

    public void CloseMenu() {
        foreach(KeyValuePair<Button, UnityAction> entry in listeners) {
            entry.Key.onClick.RemoveListener(entry.Value);
        }

        listeners.Clear();

        gameObject.SetActive(false);
    }

}
