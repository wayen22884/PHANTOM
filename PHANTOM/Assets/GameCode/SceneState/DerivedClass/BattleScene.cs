using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene:MonoBehaviour
{
    private static ReactiveProperty<bool> Pause = new ReactiveProperty<bool>();
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
        
        Tool.GetUIComponent<Button>(GameResource.Canvas, "PauseButton").onClick.AddListener(() =>
        {
            Pause.Value = !Pause.Value;
        });
        
        Pause.Subscribe(AllSourcePool.PlayerCharacter.ClickPause);
        Pause.Subscribe(AllSourcePool.ClickPause);
        Pause.Subscribe(isPause => Time.timeScale = isPause ? 0 : 1);
    }
}
