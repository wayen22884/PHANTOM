using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Main : MonoBehaviour
{
    private static string sceneName;
    
    private static ReactiveProperty<bool> pause = new ReactiveProperty<bool>();

    public static IReactiveProperty<bool> PauseEvent => pause;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        GameResource.Initialize();

        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => LoadSceneMode("StartMenu"));
    }

    
    public  static void LoadSceneMode(string sceneName,Action onLoadEnd=null)
    {
        LoadSceneReset();
        
        Debug.Log($"Load{sceneName}");
        Main.sceneName = sceneName;
        var asyncOperation= Addressables.LoadSceneAsync($"Scenes/{sceneName}",UnityEngine.SceneManagement.LoadSceneMode.Single);
        asyncOperation.Completed += _=>onLoadEnd?.Invoke();
    }

    private static void LoadSceneReset()
    {
        pause?.Dispose();
        pause = new ReactiveProperty<bool>();
    }

    public static string NowScene => sceneName;

    public static void ClickPause()
    {
        pause.Value = !pause.Value;
    }
}
