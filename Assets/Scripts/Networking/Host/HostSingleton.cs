using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{

    public HostGameManager GameManager { get; private set; }

    private static HostSingleton instance;
    public static HostSingleton Instance
    {
        get 
        { 
            if(instance !=null) 
                return instance;
            instance = FindObjectOfType<HostSingleton>();
            if(instance == null)
            {
                Debug.LogError("No host singleton in the scene");
                return null;
            }    

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();

    }


}
