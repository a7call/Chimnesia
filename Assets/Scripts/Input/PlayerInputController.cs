using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput playerInput;
    private MouseLook mouseLook;

    private InputAction lookAction;
    private InputAction zoomAction;

    private Vector2 mouseInput;
    private Vector2 zoomInput;
    private void Awake()
    {
        //References
        playerInput = GetComponent<PlayerInput>();
        mouseLook = GetComponentInChildren<MouseLook>();

        //Actions
        zoomAction = playerInput.actions["Zoom"];
        lookAction = playerInput.actions["Look"];


    }
    private void OnEnable()
    {
        lookAction.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        zoomAction.performed += ctx => zoomInput = ctx.ReadValue<Vector2>();
    }
    private void Update()
    {
        mouseLook.GetZoomInputs(zoomInput);
        mouseLook.GetInputs(mouseInput);
    }
}
