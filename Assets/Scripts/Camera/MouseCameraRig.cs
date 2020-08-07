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

    public Transform cursorPosition;
    public Transform lockPosition;
    public Transform pitch;
    public Quaternion targetPitchRotation;

    public Vector3 targetPosition;
    public Camera camera;

    public float tiltSensitivity = 20f;
    public float smooth = 5f;

    // Start is called before the first frame update
    void Start()
    {
        max_x = Screen.width;
        max_y = Screen.height;
        camera_speed = 0.1f;  // TODO: Get this value from user config
        lockCameraSpeed = 2;
        targetPosition = transform.position;
    }

    void moveCamMouse()
    {
        float y = Input.GetAxisRaw("Horizontal2") * tiltSensitivity;

        if(y != 0) {
            targetPitchRotation = Quaternion.Euler(pitch.localEulerAngles.x, pitch.localEulerAngles.y + y, pitch.localEulerAngles.z);
        }

        pitch.rotation = Quaternion.Lerp(pitch.rotation, targetPitchRotation,  Time.deltaTime * smooth);

        if (!lockToPos)
        {
            var mouse_x = Input.mousePosition.x;
            var mouse_y = Input.mousePosition.y;
            if (mouse_x > max_x - border_size && mouse_x < max_x)
            {
                targetPosition += new Vector3(camera.transform.right.x * camera_speed, 0, camera.transform.right.z * camera_speed);
            }
            else if (mouse_x < 0 + border_size && mouse_x > 0)
            {
                targetPosition -= new Vector3(camera.transform.right.x * camera_speed, 0, camera.transform.right.z * camera_speed);
            }
            if (mouse_y > max_y - border_size && mouse_y < max_y)
            {
                targetPosition += new Vector3(camera.transform.forward.x * camera_speed, 0, camera.transform.forward.z * camera_speed);
            }
            else if (mouse_y < 0 + border_size && mouse_y > 0)
            {
                targetPosition -= new Vector3(camera.transform.forward.x * camera_speed, 0, camera.transform.forward.z * camera_speed);
            }
        } else {
            targetPosition = lockPosition.position;
        }
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, lockCameraSpeed * Time.deltaTime);

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