using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool gamepadControls;
    public float mouseThrottleCap = .2f;
    float mouseThrottle = 0;
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");
    public static event EventHandler<InfoEventArgs<Coord>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    public static event EventHandler<InfoEventArgs<Vector3>> mouseMoveEvent;

    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gamepadControls)
        {
            int x = _hor.Update();
            int y = _ver.Update();
            if (x != 0 || y != 0)
            {
                if (moveEvent != null)
                {
                    moveEvent(this, new InfoEventArgs<Coord>(new Coord(x, y)));
                }
            }
        }
        else
        {
            mouseThrottle += Time.deltaTime;
            // TODO: profile with mouse throttle and without
            if (mouseMoveEvent != null)
            {
                if(mouseThrottle >= mouseThrottleCap) {
                    mouseThrottle = 0;
                    mouseMoveEvent(this, new InfoEventArgs<Vector3>(Input.mousePosition));
                }
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                {
                    fireEvent(this, new InfoEventArgs<int>(i));
                }
            }
        }
    }
}
