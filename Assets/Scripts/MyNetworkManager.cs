using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MyNetworkManager : NetworkManager
{
    public GameObject serverObject;

    public override void OnStartServer()
    {
        Application.targetFrameRate = 30;
        serverObject = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "serverObject"));
        NetworkServer.Spawn(serverObject);
        Debug.Log("Server Started!");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server Stopped!");
    }

    public override void OnClientConnect()
    {
        Application.targetFrameRate = 30;
        Debug.Log("Connected to Server");
    }

    public override void OnClientDisconnect()
    {
        Debug.Log("Disconnected from Server");
    }

    private void loadServerObject()
    {
        /*
        ObjectData objectData =  SaveSystem.LoadObjectPosition();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (objectData != null)
        {
            cube.transform.position = new Vector3(objectData.position[0], objectData.position[1], objectData.position[2]);
        }
        //serverObjects.Add(cube);
        */
    }
}
