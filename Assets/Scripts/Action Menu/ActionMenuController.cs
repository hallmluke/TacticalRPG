using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenuController : MonoBehaviour
{

    const string ShowKey = "Show";
    const string HideKey = "Hide";
    const string EntryPoolKey = "ActionMenuPanel.Entry";
    const int MenuCount = 4;

    [SerializeField] GameObject entryPrefab;
    [SerializeField] Text titleLabel;
    [SerializeField] LayoutAnchorPanel panel;
    [SerializeField] GameObject canvas;
    List<ActionMenuEntry> menuEntries = new List<ActionMenuEntry>(MenuCount);
    public int selection { get; private set; }

    void Awake() {
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    ActionMenuEntry Dequeue() {
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        ActionMenuEntry entry = p.GetComponent<ActionMenuEntry>();
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true);
        entry.Reset();
        entry.actionMenuController = this;
        return entry;
    }
    void Enqueue(ActionMenuEntry entry) {
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    void Clear() {
        for (int i = menuEntries.Count - 1; i >= 0; --i) {
            Enqueue(menuEntries[i]);
        }
        menuEntries.Clear();
    }

    Tweener TogglePos(string pos) {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseOutQuad;
        return t;
    }

    bool SetSelection(int value) {
        if (menuEntries[value].IsLocked) {
            return false;
        }
        // Deselect the previously selected entry
        if (selection >= 0 && selection < menuEntries.Count) {
            menuEntries[selection].IsSelected = false;
        }

        selection = value;

        // Select the new entry
        if (selection >= 0 && selection < menuEntries.Count) {
            menuEntries[selection].IsSelected = true;
        }

        return true;
    }

    public bool SetSelection(ActionMenuEntry entry) {
        for (int i = 0; i < menuEntries.Count; i++) {
            if (entry == menuEntries[i]) {
                return SetSelection(i);
            }
        }

        return false;
    }

    public void Next()
    {
        for (int i = selection + 1; i < selection + menuEntries.Count; ++i) {
            int index = i % menuEntries.Count;
            if (SetSelection(index)) {
                break;
            }
        }
    }
    public void Previous()
    {
        for (int i = selection - 1 + menuEntries.Count; i > selection; --i) {
            int index = i % menuEntries.Count;
            if (SetSelection(index)) {
                break;
            }
        }
    }

    public void Show(string title, List<string> options) {
        canvas.SetActive(true);
        Clear();
        titleLabel.text = title;
        for (int i = 0; i < options.Count; ++i) {
            ActionMenuEntry entry = Dequeue();
            entry.Title = options[i];
            menuEntries.Add(entry);
        }
        SetSelection(0);
        TogglePos(ShowKey);
    }

    public void SetLocked(int index, bool value) {
        if (index < 0 || index >= menuEntries.Count) {
            return;
        }
        menuEntries[index].IsLocked = value;
        if (value && selection == index) {
            Next();
        }
    }

    public void Hide() {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate (object sender, System.EventArgs e) {
            if (panel.CurrentPosition == panel[HideKey]) {
                Clear();
                canvas.SetActive(false);
            }
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
