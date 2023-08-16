using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class Host : MonoBehaviour
{

    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        if (networkManager == null)
        {
            Debug.Log("Error al encontrar al network manager");
            return;
        }
    }

    public void StartHost()
    {
        if (networkManager != null)
        {
            networkManager.StartHost();
        }
        else
        {
            Debug.Log("Error al iniciar el host");
        }
    }

    public void StartClient()
    {
        if (networkManager != null)
        {
            networkManager.StartClient();
        }
        else
        {
            Debug.Log("Error al iniciar el host");
        }
    }
}
