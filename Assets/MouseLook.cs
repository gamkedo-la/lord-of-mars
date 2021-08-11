using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 50f;

    public Transform playerBody;

    float xRotation = 0f;
    public float cameraTilt = 0.0f;
    private float cameraTiltSmooth = 0.0f;
    private const float TILT_DEGREES = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, cameraTiltSmooth * TILT_DEGREES);
        playerBody.Rotate(Vector3.up * mouseX); 
    }


    private void FixedUpdate()
    {
        float rate = 0.1f;
        cameraTiltSmooth = cameraTilt * rate + cameraTiltSmooth * (1.0f - rate);
    }
}
