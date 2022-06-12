using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene:MonoBehaviour
{
    [SerializeField] private enemyGenerateMgr enemyGenerateMgr;
    private void Awake()
    {
        GameResource.BattleSceneInitialize();
        Tool.GetUIComponent<Button>(GameResource.Canvas, "LoseButton").onClick.AddListener(() =>
        {
            GameEnd(false);
        });
        Tool.GetUIComponent<Button>(GameResource.Canvas, "EnemyGenerateButton").onClick.AddListener(() =>
        {
            enemyGenerateMgr.GenerateNextEnemy();
        });
        Tool.GetUIComponent<Button>(GameResource.Canvas, "StartBattleButton").onClick.AddListener(() =>
        {
            enemyGenerateMgr.StartBattle();
        });
        
        Tool.GetUIComponent<Button>(GameResource.Canvas, "PauseButton").onClick.AddListener(Main.ClickPause);
        
        Main.PauseEvent.Subscribe(AllSourcePool.PlayerCharacter.ClickPause);
        Main.PauseEvent.Subscribe(AllSourcePool.ClickPause);
        Main.PauseEvent.Subscribe(isPause => Time.timeScale = isPause ? 0 : 1);
        Main.PauseEvent.Subscribe(TimeEventCheck.Pause);
    }

    private void Update()
    {
        TimeEventCheck.TimeUpdate();
    }

    public static void GameEnd(bool win)
    {
        if (win)
        {
            GameComplete();
        }
        else
        {
            GameOver();
        }
    }

    private static void GameOver()
    {
        AllSourcePool.Clear();
        Main.LoadSceneMode("StartMenu");
    }

    private static void GameComplete()
    {
        GameOver();
    }
}
