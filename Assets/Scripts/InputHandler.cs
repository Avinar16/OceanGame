using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler InputManager { get; private set; }
    public PlayerInputActions Input { get; private set; }

    private void Awake()
    {
        if (InputManager != null && InputManager != this)
        {
            Destroy(this);
        }
        else
        {
            InputManager = this;
        }



        Input = new PlayerInputActions();
    }

    // Update is called once per frame
    public void SetPlayerControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Input.Disable();
        Input.Player.Enable();
    }
    public void SetUiControls()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Input.Disable();
        Input.UI.Enable();
    }
}
