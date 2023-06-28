using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]private Transform PlayerTransform;
    [SerializeField] private Transform CameraBrain;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerTransform.localRotation = Quaternion.Euler(0, CameraBrain.eulerAngles.y, 0f);
    }
}
