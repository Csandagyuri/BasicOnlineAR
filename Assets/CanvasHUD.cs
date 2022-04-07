using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHUD : MonoBehaviour
{
    [SerializeField]
    public GameObject PanelStart, PanelStop, PanelReady;

    [SerializeField]
    public Button buttonHost, buttonServer, buttonClient, buttonStop, buttonReady;

    [SerializeField]
    public InputField inputFieldAddress;

    [SerializeField]
    public Text serverText;
    [SerializeField]
    public Text clientText;

    // Start is called before the first frame update
                                                       
    private void Start()
    {
        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        //Adds a listener to the main input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        //Make sure to attach these Buttons in the Inspector
        buttonHost.onClick.AddListener(ButtonHost);
        buttonServer.onClick.AddListener(ButtonServer);
        buttonClient.onClick.AddListener(ButtonClient);
        buttonStop.onClick.AddListener(ButtonStop);
        buttonReady.onClick.AddListener(ButtonReady);

        //This updates the Unity canvas, we have to manually call it every change, unlike legacy OnGUI.
        SetupCanvas();
    }

    private void ButtonReady()
    {
        NetworkClient.Ready();
        if (NetworkClient.localPlayer == null)
        {
            NetworkClient.AddPlayer();
        }
    }

    // Invoked when the value of the text field changes.
    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
        clientReadyPanel();
    }

    public void ButtonServer()
    {
        NetworkManager.singleton.StartServer();
        clientReadyPanel();
    }

    public void ButtonClient()
    {
        NetworkManager.singleton.StartClient();
        clientReadyPanel();
    }

    public void ButtonStop()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

        SetupCanvas();
    }

    public void clientReadyPanel()
    {

        PanelStart.SetActive(false);
        PanelReady.SetActive(true);

    }

    public void ingamePanel()
    {
        PanelStart.SetActive(false);
        PanelStop.SetActive(true);
    }

    public void SetupCanvas()
    {
        PanelReady.SetActive(false);
        PanelStart.SetActive(true);
        PanelStop.SetActive(false);

        /*
        // Here we will dump majority of the canvas UI that may be changed.

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            
            if (NetworkClient.active)
            {
                PanelStart.SetActive(false);
                PanelStop.SetActive(true);
                clientText.text = "Connecting to " + NetworkManager.singleton.networkAddress + "..";
            }
            else
            {
                PanelStart.SetActive(true);
                PanelStop.SetActive(false);
            }
        }
        else
        {
            clientReadyPanel();

            
            PanelStart.SetActive(false);
            PanelStop.SetActive(true);

            // server / client status message
            if (NetworkServer.active)
            {
                serverText.text = "Server: active. Transport: " + Transport.activeTransport;
            }
            if (NetworkClient.isConnected)
            {
            }
            
        }
        */
    }
}
