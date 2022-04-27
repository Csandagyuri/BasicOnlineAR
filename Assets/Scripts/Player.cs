using UnityEngine;
using Mirror;
using UnityEngine.XR.ARFoundation;

public class Player : NetworkBehaviour
{
    private ARSessionOrigin arSessionOrigin;
    private GameObject serverObj;

    private void Awake()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    private void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        if (Application.platform == RuntimePlatform.Android)
        {
            if (isLocalPlayer)
            {
                //#if UNITY_S
                ChangeToARCommand();
            }
        }
    }

    [Command]
    private void ChangeToARCommand()
    {
        //Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Terrain").GetComponent<Collider>());
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
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (isLocalPlayer)
            {
                arSessionOrigin.camera.transform.position = transform.position + new Vector3(0f, 1.5f, -3f);
                arSessionOrigin.camera.transform.LookAt(transform);
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                
                Vector3 movement = new Vector3(x * 0.1f, 0f, y * 0.1f);

                //Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Terrain").GetComponent<Collider>());

                //transform.position = transform.position + movement;
                PlayerMoveCommand(movement);
            }
        }

        
        if (Application.platform == RuntimePlatform.Android)
        {
            if (isLocalPlayer)
            {
                
                float distance = 0.5f;

                //transform.position = arSessionOrigin.camera.transform.position + arSessionOrigin.camera.transform.forward * distance;
                /*
                Vector3 cameraPosition = arSessionOrigin.camera.transform.position;
                Vector3 cameraDirection = arSessionOrigin.camera.transform.forward;
                RaycastHit hit;
                if (Physics.Raycast(cameraPosition, cameraDirection, out hit, distance * 1.2f) && hit.collider.tag == "Terrain")
                {
                    Vector3 step = hit.collider.transform.position + gameObject.transform.lossyScale - transform.position;
                        PlayerMoveCommandWithPosition(step, transform.position, new Vector3(0,0,0));
                }else
                {
                    PlayerMoveCommandWithPosition(arSessionOrigin.camera.transform.position + arSessionOrigin.camera.transform.forward * distance - transform.position, transform.position, arSessionOrigin.camera.transform.forward);
                }

                */
                PlayerMoveCommandWithPosition(arSessionOrigin.camera.transform.position + arSessionOrigin.camera.transform.forward * distance - transform.position, transform.position, arSessionOrigin.camera.transform.forward);
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
