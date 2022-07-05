using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene:MonoBehaviour
{
    [SerializeField] private enemyGenerateMgr enemyGenerateMgr;
    [SerializeField] private PlayAnimation showDead;
    [SerializeField] private TextMeshProUGUI clickReturnMainMenu;

    private SettingUI settingUI;
    public static BattleScene Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        showDead.OnClickCollider += ChangeStartScene;
        showDead.OnShowSpriteEnd += ()=>clickReturnMainMenu.gameObject.SetActive(true);
        GameResource.BattleSceneInitialize();

        MusicSystem.Instance.StartBGM = GameResource.BattleStartBGM;
        var loopList= new List<AudioClip>();
        loopList.Add(GameResource.BattleLoopBGM);
        MusicSystem.Instance.LoopBGMs = loopList;
        MusicSystem.Instance.PlayMusicAndLoop();
        var testRoot=Tool.FindChildGameObject(GameResource.Canvas, "TestStatus");
        Tool.GetUIComponent<Button>(testRoot, "LoseButton").onClick.AddListener(() =>
        {
            GameEnd(false);
        });
        Tool.GetUIComponent<Button>(testRoot, "WinButton").onClick.AddListener(() =>
        {
            GameEnd(true);
        });
        Tool.GetUIComponent<Button>(testRoot, "EnemyGenerateButton").onClick.AddListener(() =>
        {
            enemyGenerateMgr.GenerateNextEnemy();
        });
        Tool.GetUIComponent<Button>(testRoot, "StartBattleButton").onClick.AddListener(() =>
        {
            enemyGenerateMgr.StartBattle();
        });
        
        Tool.GetUIComponent<Button>(testRoot, "PauseButton").onClick.AddListener(Main.ClickPause);


        settingUI = new SettingUI();
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
        //PlayMusic(GameResource.FailureBGM);
        MusicSystem.Instance.StopMusic();
        MusicSystem.Instance.PlayBGV(GameResource.FailureBGM);
        showDead.ShowSprite();
    }

    private static void ChangeStartScene()
    {
        ChangeScene("StartMenu");
    }

    private static void ChangeScene(string sceneName,Action onLoadEnd=null)
    {
        AllSourcePool.Clear();
        Main.LoadSceneMode(sceneName,onLoadEnd);
    }

    private void GameComplete()
    {
        ChangeScene("End",() => PlayMusic(GameResource.VictoryBGM));
    }

    void PlayMusic(AudioClip BGM)
    {
        MusicSystem.Instance.StartBGM = BGM;
        var loopList= new List<AudioClip>();
        loopList.Add(BGM);
        MusicSystem.Instance.LoopBGMs = loopList;
        MusicSystem.Instance.PlayMusicAndLoop();
    }
    
    
}
