using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Main : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this);
        GameResource.Initialize();
        
        LoadSceneMode("StartMenu");
    }

    
    public  static void LoadSceneMode(string sceneName,Action onLoadEnd=null)
    {
        Debug.Log($"Load{sceneName}");
        var asyncOperation= Addressables.LoadSceneAsync($"Scenes/{sceneName}",UnityEngine.SceneManagement.LoadSceneMode.Single);
        asyncOperation.Completed += _=>onLoadEnd?.Invoke();
    }
}
