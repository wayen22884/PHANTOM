using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene:MonoBehaviour
{
    [SerializeField] private EnemyGenerate enemyGenerate;
    private void Awake()
    {
        GameResource.BattleSceneInitialize();
        Tool.GetUIComponent<Button>(GameResource.Canvas, "LoseButton").onClick.AddListener(() =>
        {
            AllSourcePool.Clear();
            Main.LoadSceneMode("StartMenu");
        });
        Tool.GetUIComponent<Button>(GameResource.Canvas, "EnemyGenerateButton").onClick.AddListener(() =>
        {
            enemyGenerate.GenerateEnemy();
        });
        
        Tool.GetUIComponent<Button>(GameResource.Canvas, "PauseButton").onClick.AddListener(Main.ClickPause);
        
        Main.PauseEvent.Subscribe(AllSourcePool.PlayerCharacter.ClickPause);
        Main.PauseEvent.Subscribe(AllSourcePool.ClickPause);
        Main.PauseEvent.Subscribe(isPause => Time.timeScale = isPause ? 0 : 1);
    }
}
