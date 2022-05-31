using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu :MonoBehaviour
{
    public string Name { get; }

    private MainMenuUI mainMenuUI;
    private SettingUI settingUI;
    public StartMenu()
    {
        Name = GetType().ToString();
    }

    private void Awake()
    {
        GameResource.MainMenuSceneInitialize();
        mainMenuUI = new MainMenuUI();
        settingUI = new SettingUI();
    }
}