using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene:MonoBehaviour
{
    private void Awake()
    {
        GameResource.BattleSceneInitialize();
        Tool.GetUIComponent<Button>(GameResource.Canvas, "LoseButton").onClick.AddListener(() =>
        {
            AllSourcePool.Clear();
            Main.LoadSceneMode("StartMenu");
        });
    }
}
