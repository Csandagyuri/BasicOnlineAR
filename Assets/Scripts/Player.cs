using UnityEngine;
using Mirror;
using UnityEngine.XR.ARFoundation;

public class Player : NetworkBehaviour
{
    private ARSessionOrigin arSessionOrigin;

    private void Awake()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        if (Application.platform == RuntimePlatform.Android)
        {
//#if UNITY_S
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
//#endif 

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
                arSessionOrigin.camera.transform.position = transform.position + new Vector3(0f, 1.5f, -3f);
                arSessionOrigin.camera.transform.LookAt(transform);
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                Vector3 movement = new Vector3(x * 0.1f, 0f, y * 0.1f);
                //transform.position = transform.position + movement;
                PlayerMoveCommand(movement);
            }
        }

        
        if (Application.platform == RuntimePlatform.Android)
        {
            if (isLocalPlayer)
            {
                
                float distance = 0.5f;

                transform.position = arSessionOrigin.camera.transform.position + arSessionOrigin.camera.transform.forward * distance;
                //PlayerMoveCommand(arSessionOrigin.camera.transform.position + arSessionOrigin.camera.transform.forward * distance - transform.position);
            }
        }

        /*
        if (isServer)
        {
            GameObject gameObject = GameObject.Find("serverObject");
            Debug.Log(gameObject.name);

            if (gameObject == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, gameObject.transform.position) <= 0.1f)
            {
                float distanceOfObjects = Vector3.Distance(transform.position, gameObject.transform.position);
                float distanceToPush = transform.localScale.x / 2 + gameObject.transform.localScale.x / 2 - distanceOfObjects;
                Vector3 direction = transform.position - gameObject.transform.position;
                Vector3 force = direction.normalized * distanceToPush * -1f;

                gameObject.transform.position = gameObject.transform.position + force;
                pushObjectRpc(name, gameObject.transform.position);
            }
        }
        */
    }

    
    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Nem ismerte a server object tagot");
        if (other.gameObject.tag == "ServerObject") {
            if(isLocalPlayer)   pushObjectCommand(other.gameObject.name);
            //isPushed = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ServerObject")
        {
            if (isLocalPlayer) pushObjectCommand(other.gameObject.name);
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

        Debug.Log(gameObject);
        //Ellenõrizni, hogy esetleg nem egy kliensen rosszul beállított pozició szerint ütközött
        if (Vector3.Distance(transform.position, gameObject.transform.position) <= 0.1f) {
            float distanceOfObjects = Vector3.Distance(transform.position, gameObject.transform.position);
            float distanceToPush = transform.localScale.x / 2 + gameObject.transform.localScale.x / 2 - distanceOfObjects;
            Vector3 direction = transform.position - gameObject.transform.position;
            Vector3 force = direction.normalized * distanceToPush * -1f * 3f;

            if (force )

            gameObject.GetComponent<Rigidbody>().AddForce(force);
            Debug.Log(force);
            pushObjectRpc(name ,gameObject.transform.position, force);
        

        }
    }
    

    [ClientRpc]
    void pushObjectRpc(string name, Vector3 position, Vector3 force)
    {
        Debug.Log(position);
        GameObject.Find(name).gameObject.GetComponent<Rigidbody>().AddForce(force);
    }
    */


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
