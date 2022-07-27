using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : IUserInterface
{
    private Button _Setting;

    public SettingUI()
    {
        Initialize();
    }
    
    
    //private GameObject _RootSetting;
    private Slider _BGM;
    private Slider _Effect;
    private EventTrigger effectEventTrigger;
    private Button _SettingHide;
    private Button _BackToMainMenu;
    private Button _Exit;
    private Toggle _FullScreen;
    private Dropdown _ScreenSize;

    private GameObject menu;
    private SettingAnimation settingAnimation;
    public override void Initialize()
    {
        //開啟Setting按鈕
        _Setting = Tool.GetUIComponent<Button>(GameResource.Canvas, "Setting");
        _Setting.onClick.AddListener(()=>MusicSystem.Instance.PlayBGV(GameResource.Button_Option));
        _Setting.onClick.AddListener(ClickSetting);
        if (Main.NowScene == "BattleScene") _Setting.onClick.AddListener(Main.ClickPause);

        //Setting內部
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "SettingStatus");

        menu = Tool.FindChildGameObject(GameResource.Canvas, "Menu");
        var BG = Tool.FindChildGameObject(GameResource.Canvas,"AnimationBG");
        settingAnimation = BG.GetComponent(typeof(SettingAnimation)) as SettingAnimation;
        settingAnimation.MenuStatusCallBack += MenuStatusCallBackClick;
        settingAnimation.rootUICallBack += RootUICallBack;
        settingAnimation.Interactable += interactable=> _Setting.interactable=interactable;
        
        SaveSystem.Load(out  Volume data);
        _BGM = Tool.GetUIComponent<Slider>(_RootUI, "BGM");
        

        
        _BGM.value = ValueChanged_AudioMixer_To_UI(data.BGM);
        _BGM.onValueChanged.AddListener(ChangeBGM);
        GameResource.BGMGroup.audioMixer.SetFloat("BGMVol", data.BGM);
        //GameResource.BGMGroup.audioMixer.GetFloat("BGMVol",out float s);
        
        
        _Effect = Tool.GetUIComponent<Slider>(_RootUI, "Effect");
        GameResource.BGMGroup.audioMixer.SetFloat("EffectVol", data.Effect);
        _Effect.value =ValueChanged_AudioMixer_To_UI(data.Effect);

        var trigger= _Effect.gameObject.AddComponent<EventTrigger>();

        _BackToMainMenu=Tool.GetUIComponent<Button>(_RootUI, "BackToMainMenu");
        _BackToMainMenu.gameObject.SetActive(Main.NowScene == "BattleScene");
        if (Main.NowScene == "BattleScene")
        {
            _BackToMainMenu.onClick.AddListener(() =>
            {
                AllSourcePool.Clear();
                Main.ClickPause();
                Main.LoadSceneMode("StartMenu");
            });
        }
        
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener( _=>ChangeEffect(_Effect.value));
        trigger.triggers.Add(entry);
        _SettingHide = Tool.GetUIComponent<Button>(_RootUI, "SettingHide");
        _SettingHide.onClick.AddListener(ClickSetting);
        if (Main.NowScene == "BattleScene") _SettingHide.onClick.AddListener(()=>MusicSystem.Instance.PlayBGV(GameResource.Button_No));
        if (Main.NowScene == "BattleScene") _SettingHide.onClick.AddListener(Main.ClickPause);
        
        //_BackToMainMenu = Tool.GetUIComponent<Button>(_RootUI, "BackToMainMenuButton");
        // if (GameLoop.Instance.SceneState == typeof(MainMenuState).ToString()) _BackToMainMenu.gameObject.SetActive(false);
        // else _BackToMainMenu.gameObject.SetActive(true);
        // _BackToMainMenu.onClick.AddListener(GameLoop.Instance.TransformToMainScene);
        // _BackToMainMenu.onClick.AddListener(GameLoop.Instance.PauseClick);

        // _Exit = Tool.GetUIComponent<Button>(_RootUI, "ExitButton");
        // if (GameLoop.Instance.SceneState == typeof(MainMenuState).ToString()) _Exit.gameObject.SetActive(false);
        // else _Exit.gameObject.SetActive(true);
        // _Exit.onClick.AddListener(Application.Quit);

        // _FullScreen = Tool.GetUIComponent<Toggle>(_RootUI, "FullScreenButton");
        // _FullScreen.isOn = Screen.fullScreen;
        // _FullScreen.onValueChanged.AddListener(CHangeFullScreen);
        //
        // _ScreenSize = Tool.GetUIComponent<Dropdown>(_RootUI, "ScreenSize");
        // _ScreenSize.onValueChanged.AddListener(ChangScreenSize);
        // IntializeScreensize(_ScreenSize);
    }

    private  void RootUICallBack(bool value)
    {
        if (IsVisable()!=value)
        {
            RootClick();
        }
    }

    private void ClickSetting()
    {
        MusicSystem.Instance.PlayBGV(GameResource.OpenSettingPage);
        if (IsVisable())
        {
            settingAnimation.Close();
        }
        else
        {
            settingAnimation.PlayAnimation();
        }
    }

    void IntializeScreensize(Dropdown dropdown)
    {
        SaveSystem.Load(out ScreenData screenData);
        Screen.SetResolution(screenData.width,screenData.height,screenData.fullscene);
        int wid = screenData.width;
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            string[] size = dropdown.options[i].text.Split('*');
            if (int.Parse(size[0]) == wid)
            {
                dropdown.value = i;
                break;
            }
        }

    }


    void ChangScreenSize(int value)
    {
        string option = _ScreenSize.options[value].text;
        string[] size = option.Split('*');
        ScreenData screenData = new ScreenData();
        screenData.width = int.Parse(size[0]);
        screenData.height = int.Parse(size[1]);
        screenData.fullscene = Screen.fullScreen;
        Screen.SetResolution(screenData.width,screenData.height,screenData.fullscene);
        SaveSystem.Save(screenData);
    }
    void CHangeFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
    void ChangeBGM(float value)
    {
        float result= ValueChanged_UI_To_AudioMixer(value);
        GameResource.BGMGroup.audioMixer.SetFloat("BGMVol", result);
        SaveSystem.Load(out  Volume data);
        data.BGM = result;
        SaveSystem.Save(data);
    }
    void ChangeEffect(float value)
    {
        MusicSystem.Instance.PlayBGV(GameResource.TestSFX);
        float result = ValueChanged_UI_To_AudioMixer(value);
        GameResource.BGVGroup.audioMixer.SetFloat("EffectVol",result );
        SaveSystem.Load(out  Volume data);
        data.Effect = result;
        SaveSystem.Save(data);
    }
    float ValueChanged_UI_To_AudioMixer(float value)
    {
        if (value >= 0.2f) return value * 40 - 20;
        else return value * 340 - 80;
    }
    float ValueChanged_AudioMixer_To_UI(float value)
    {
        if (value >= 0.2f) return (value + 20) / 40;
        else return (value + 80) / 340;
    }


    private void MenuStatusCallBackClick(bool enable)
    {
        menu.SetActive(enable);
    }
}