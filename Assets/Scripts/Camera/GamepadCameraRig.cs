using UnityEngine;
using System.Collections;
public class GamepadCameraRig : MonoBehaviour
{
    public float speed = 3f;
    public Transform follow;
    Transform _transform;

    public Transform pitch;
    public Quaternion targetPitchRotation;

    public float tiltSensitivity = 20f;
    public float smooth = 5f;

    void Awake() {
        _transform = transform;
    }

    void Update() {
        if (follow) {
            _transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
        }

        float y = Input.GetAxisRaw("Horizontal2") * tiltSensitivity;

        if(y != 0) {
            targetPitchRotation = Quaternion.Euler(pitch.localEulerAngles.x, pitch.localEulerAngles.y + y, pitch.localEulerAngles.z);
        }

        pitch.rotation = Quaternion.Lerp(pitch.rotation, targetPitchRotation,  Time.deltaTime * smooth);
    }
}