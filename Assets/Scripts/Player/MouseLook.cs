using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float sensX = 100f;
    [SerializeField] float sensY = 100f;
    //[SerializeField] float sensZoom = 10f;

    [SerializeField] Transform playerCamera;

    float mouseX, mouseY, mouseZoom;
    float minFov = 30;
    float maxFov = 60;

    float multiplier = 0.01f;

    float xRotation = 0f;
    float yRotation;

    [SerializeField] Transform orientation;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        //yRotation += mouseX * sensX * multiplier;
        //xRotation -= mouseY * sensY * multiplier;

        //xRotation = Mathf.Clamp(xRotation, -30f, 30f);
        //yRotation = Mathf.Clamp(yRotation, -30f, 30f);

        //playerCamera.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        //orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void GetInputs(Vector2 mouseLook)
    {
        mouseX = mouseLook.x;
        mouseY = mouseLook.y;
    }
    public void GetZoomInputs(Vector2 mouseZoom)
    {
        this.mouseZoom = mouseZoom.y;
    }
}
