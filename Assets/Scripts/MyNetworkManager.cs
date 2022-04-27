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
}
