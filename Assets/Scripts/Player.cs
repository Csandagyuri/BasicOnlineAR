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

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (isLocalPlayer)
            {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                float z;
                if( Input.GetKeyDown("left shift"))
                {
                    z = -0.1f;
                } else if(Input.GetKeyDown("space")){
                    z = 0.1f;
                }else
                {
                    z = 0f;
                }
                Vector3 movement = new Vector3(x * 0.1f, z * 0.1f, y * 0.1f);
                //transform.position = transform.position + movement;
                PlayerMoveCommand(movement);

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

    
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Nem ismerte a server object tagot");
        if (other.gameObject.tag == "ServerObject") {
            pushObjectCommand(other.gameObject.name);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ServerObject")
        {
            pushObjectCommand(other.gameObject.name);
        }
    }
    

    [Command]
    private void pushObjectCommand(string name)
    {
        GameObject gameObject = GameObject.Find(name);
        if(gameObject == null)
        {
            Debug.Log("returned with null");
            return;
        }

        //Ellenõrizni, hogy esetleg nem egy kliensen rosszul beállított pozició szerint ütközött
        if (Vector3.Distance(transform.position, gameObject.transform.position) <= 0.1f) {
            float distanceOfObjects = Vector3.Distance(transform.position, gameObject.transform.position);
            float distanceToPush = transform.localScale.x / 2 + gameObject.transform.localScale.x / 2 - distanceOfObjects;
            Vector3 direction = transform.position - gameObject.transform.position;
            Vector3 force = direction.normalized * distanceToPush * -1f;

            gameObject.transform.position = gameObject.transform.position + force;
            Debug.Log(force);
            pushObjectRpc(name ,gameObject.transform.position);

        }
    }

    [ClientRpc]
    void pushObjectRpc(string name, Vector3 position)
    {
        Debug.Log(position);
        GameObject.Find(name).gameObject.transform.position = position;
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

}
