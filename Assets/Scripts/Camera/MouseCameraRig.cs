using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraRig : MonoBehaviour
{
    private float camera_speed;
    private float lockCameraSpeed;
    private int border_size = 50;  // pixels around border that trigger movement
    private int max_x;
    private int max_y;

    public bool lockToPos;

    public Transform lockPosition;

    // Start is called before the first frame update
    void Start()
    {
        max_x = Screen.width;
        max_y = Screen.height;
        camera_speed = 0.05f;  // TODO: Get this value from user config
        lockCameraSpeed = 2;
    }

    void moveCamMouse()
    {
        if (!lockToPos)
        {
            var mouse_x = Input.mousePosition.x;
            var mouse_y = Input.mousePosition.y;
            if (mouse_x > max_x - border_size && mouse_x < max_x)
            {
                transform.position = transform.position + new Vector3(camera_speed, 0, 0);
            }
            else if (mouse_x < 0 + border_size && mouse_x > 0)
            {
                transform.position = transform.position - new Vector3(camera_speed, 0, 0);
            }
            if (mouse_y > max_y - border_size && mouse_y < max_y)
            {
                transform.position = transform.position + new Vector3(0, 0, camera_speed);
            }
            else if (mouse_y < 0 + border_size && mouse_y > 0)
            {
                transform.position = transform.position - new Vector3(0, 0, camera_speed);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, lockPosition.position, lockCameraSpeed * Time.deltaTime);
        }
    }

    // Moves the camera back to where it was at the start of the turn
    void restoreCameraPosition()
    {
        //transform.position = game_state.cam_position;
    }

    // Update is called once per frame
    void Update()
    {
        moveCamMouse();
        /*if (Input.GetButtonDown("Reset Camera")) {
            restoreCameraPosition();
        }*/
    }
}