using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR.ARFoundation;

public class Player : NetworkBehaviour
{
    private ARSessionOrigin arSessionOrigin;

    private void Awake()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    void HandleMovement()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (isLocalPlayer)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
                Vector3 movement = new Vector3(moveHorizontal * 0.1f, moveVertical * 0.1f, 0);
                transform.position = transform.position + movement;
            }
        }


        if (Application.platform == RuntimePlatform.Android)
        {
            if (isLocalPlayer)
            {
                transform.position = arSessionOrigin.camera.transform.position;
            }
        }

    }

    private void Update()
    {
        HandleMovement();
    }

}
