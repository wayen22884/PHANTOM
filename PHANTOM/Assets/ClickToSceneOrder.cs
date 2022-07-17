using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickToSceneOrder : MonoBehaviour
{
    [SerializeField] private SceneLoader loader;
    [SerializeField] private Button button;
    public void OpenBackToMainMenuButton()
    {
        button.onClick.AddListener(()=>loader.ChangeScene());
        button.interactable = true;
    }
}
