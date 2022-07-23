using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prologue : MonoBehaviour
{
    [SerializeField]private ClickToSceneOrder clickToSceneOrder;

    public static string isRead="isRead";
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(isRead))
        {
            clickToSceneOrder.OpenBackToMainMenuButton();
        }
    }

    public void SaveKey()
    {
        PlayerPrefs.SetString(isRead, isRead);
    }
}
