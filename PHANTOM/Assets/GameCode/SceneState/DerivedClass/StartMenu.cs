using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private PlayAnimation playAnimation;
    [SerializeField] private Image image;
    public string Name { get; }
    public string ChangeScene;

    private MainMenuUI mainMenuUI;
    private SettingUI settingUI;
    
    public StartMenu()
    {
        Name = GetType().ToString();
    }

    private void Awake()
    {
        GameResource.MainMenuSceneInitialize();
        mainMenuUI = new MainMenuUI(() =>
        {
            settingUI.CloseSetting();
            image.gameObject.SetActive(false);
            playAnimation.gameObject.SetActive(true);
            playAnimation.Play();
        });
        settingUI = new SettingUI();
        if (PlayerPrefs.HasKey(BattleScene.winPrefs))
        {
            MusicSystem.Instance.StartBGM = GameResource.TitleStartBGM;
            var loopList = new List<AudioClip>();
            loopList.Add(GameResource.TitleLoopBGM);
            MusicSystem.Instance.LoopBGMs = loopList;            
        }
        else
        {
            MusicSystem.Instance.StartBGM = GameResource.TitleStartBGM;
            var loopList = new List<AudioClip>();
            loopList.Add(GameResource.TitleLoopBGM);
            MusicSystem.Instance.LoopBGMs = loopList;            
        }
        MusicSystem.Instance.PlayMusicAndLoop();


        playAnimation.OnPlayAnimationEnd += () => Main.LoadSceneMode(ChangeScene, () => MusicSystem.Instance.PlayMusic(GameResource.StoryPrologue));
    }
}