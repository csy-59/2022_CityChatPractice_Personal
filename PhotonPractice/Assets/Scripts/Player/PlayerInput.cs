using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public bool Shift { get; private set; }
    private void resetKeys()
    {
        X = Y = 0;
        Shift = false;
    }

    private void Awake()
    {
        resetKeys();
    }

    private void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        resetKeys();

        X = Input.GetAxis("Vertical");
        Y = Input.GetAxis("Horizontal");
       
        Shift = Input.GetKey(KeyCode.LeftShift);
    }
}
