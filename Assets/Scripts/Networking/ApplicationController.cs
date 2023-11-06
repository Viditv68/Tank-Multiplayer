using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;
    // Start is called before the first frame update
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool _isDedicatedServer)
    {   
        if(_isDedicatedServer) 
        {

        }
        else
        {
            ClientSingleton clientSingleton = Instantiate(clientPrefab);

            await clientSingleton.CreateClient();

            HostSingleton hostSingleton = Instantiate(hostPrefab);

            hostSingleton.CreateHost();

        }
    }

}
