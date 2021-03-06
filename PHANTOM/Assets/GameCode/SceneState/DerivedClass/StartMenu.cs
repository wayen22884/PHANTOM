using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private PlayAnimation playAnimation;
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
            playAnimation.gameObject.SetActive(true);
            playAnimation.Play();
        });
        settingUI = new SettingUI();
        MusicSystem.Instance.StartBGM = GameResource.TitleStartBGM;
        var loopList = new List<AudioClip>();
        loopList.Add(GameResource.TitleLoopBGM);
        MusicSystem.Instance.LoopBGMs = loopList;
        MusicSystem.Instance.PlayMusicAndLoop();


        playAnimation.OnPlayAnimationEnd += () => Main.LoadSceneMode(ChangeScene, () => MusicSystem.Instance.PlayMusic(GameResource.StoryPrologue));
    }
}