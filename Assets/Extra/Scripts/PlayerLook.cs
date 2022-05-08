using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] private float Xsens;
    [SerializeField] private float Ysens;

    [SerializeField] Transform cam;
    [SerializeField] Transform Orientation;

    float mouseX;
    float mouseY;

    float Multi = 0.01f;

    float XRotation;
    float YRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        cam.transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        Orientation.transform.rotation = Quaternion.Euler(0, YRotation, 0);
    }

    void PlayerInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        YRotation += mouseX * Xsens * Multi;
        XRotation -= mouseY * Ysens * Multi;

        XRotation = Mathf.Clamp(XRotation, -90, 90);

    }
}
