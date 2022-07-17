using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string TargetScene;
    
    public void ChangeScene()
    {
        Main.LoadSceneMode(TargetScene);
    }
}
