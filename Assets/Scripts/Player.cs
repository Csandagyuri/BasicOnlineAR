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

    private void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        #if UNITY_ANDROID
            if (isLocalPlayer)
            {
                ChangeToARCommand();
            }
        #endif
    }

    [Command]
    private void ChangeToARCommand()
    {
        
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log(GetComponent<Rigidbody>().useGravity);

        ChangeToARRPC();
    }

    [ClientRpc]
    private void ChangeToARRPC()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log(GetComponent<Rigidbody>().useGravity);
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        #if UNITY_STANDALONE_WIN
            if (isLocalPlayer)
            {
                arSessionOrigin.camera.transform.position = transform.position + new Vector3(0f, 1.5f, -3f);
                arSessionOrigin.camera.transform.LookAt(transform);
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                
                Vector3 movement = new Vector3(x * 0.1f, 0f, y * 0.1f);

                PlayerMoveCommand(movement);
            }
        #endif
        
        #if UNITY_ANDROID
            if (isLocalPlayer)
            {
                float distance = 0.5f;
                PlayerMoveCommandWithPosition(arSessionOrigin.camera.transform.position + arSessionOrigin.camera.transform.forward * distance - transform.position, transform.position, arSessionOrigin.camera.transform.forward);
            }
        #endif

    }

    [Command]
    void PlayerMoveCommand(Vector3 step)
    {
        transform.position = transform.position + step;
        PlayerMoveRPC(transform.position);
        
        
    }

    [ClientRpc]
    void PlayerMoveRPC(Vector3 position)
    {
        transform.position = position;
    }

    [Command]
    void PlayerMoveCommandWithPosition(Vector3 step, Vector3 position, Vector3 direction)
    {

        transform.position = position + step;
        transform.forward = direction;
        SetRotationRPC(direction);
        PlayerMoveRPC(transform.position);
    }

    [ClientRpc]
    void SetRotationRPC(Vector3 direction)
    {

        transform.forward = direction;
    }

}
