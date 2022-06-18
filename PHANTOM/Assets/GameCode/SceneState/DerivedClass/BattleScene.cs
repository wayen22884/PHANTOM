using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene:MonoBehaviour
{
    [SerializeField] private enemyGenerateMgr enemyGenerateMgr;
    [SerializeField] private PlayAnimation showDead;

    public static BattleScene Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        showDead.OnClickCollider += ChangeStartScene;
        GameResource.BattleSceneInitialize();
        Tool.GetUIComponent<Button>(GameResource.Canvas, "LoseButton").onClick.AddListener(() =>
        {
            GameEnd(false);
        });
        Tool.GetUIComponent<Button>(GameResource.Canvas, "WinButton").onClick.AddListener(() =>
        {
            GameEnd(true);
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

    public void GameEnd(bool win)
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

    private void GameOver()
    {
        showDead.ShowSprite();
    }

    private static void ChangeStartScene()
    {
        ChangeScene("StartMenu");
    }

    private static void ChangeScene(string sceneName)
    {
        AllSourcePool.Clear();
        Main.LoadSceneMode(sceneName);
    }

    private void GameComplete()
    {
        ChangeScene("End");
    }
}
