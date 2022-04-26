using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu :MonoBehaviour
{
    public string Name { get; }

    public StartMenu()
    {
        Name = GetType().ToString();
    }

    private void Awake()
    {
        GameResource.MainMenuSceneInitialize();
        Tool.GetUIComponent<Button>(GameResource.Canvas, "StartBattleSceneButton").onClick.AddListener(()=>Main.LoadSceneMode("BattleScene"));
    }
}