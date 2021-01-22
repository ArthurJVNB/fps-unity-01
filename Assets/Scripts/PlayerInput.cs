using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action OnJump;

    public float Vertical { get { return Input.GetAxis("Vertical"); }}
    public float Horizontal { get { return Input.GetAxis("Horizontal"); } }
    public bool IsWalking { get { return Input.GetButton("Walk"); } }

    private void Update()
    {
        if (Input.GetButtonUp("Jump")) OnJump?.Invoke();
    }
}
