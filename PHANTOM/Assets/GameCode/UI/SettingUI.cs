using UnityEngine;
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
    private Button _SettingHide;
    private Button _BackToMainMenu;
    private Button _Exit;
    private Toggle _FullScreen;
    private Dropdown _ScreenSize;
    public override void Initialize()
    {
        //開啟Setting按鈕
        _Setting = Tool.GetUIComponent<Button>(GameResource.Canvas, "Setting");
        _Setting.onClick.AddListener(RootClick);
        if (Main.NowScene == "BattleScene") _Setting.onClick.AddListener(Main.ClickPause);

        //Setting內部
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "SettingStatus");
        
        
        SaveSystem.Load(out  Volume data);
        _BGM = Tool.GetUIComponent<Slider>(_RootUI, "BGM");
        

        
        _BGM.value = ValueChanged_AudioMixer_To_UI(data.BGM);
        _BGM.onValueChanged.AddListener(ChangeBGM);
        GameResource.BGMGroup.audioMixer.SetFloat("BGMVol", data.BGM);
        //GameResource.BGMGroup.audioMixer.GetFloat("BGMVol",out float s);
        
        
        _Effect = Tool.GetUIComponent<Slider>(_RootUI, "Effect");
        GameResource.BGMGroup.audioMixer.SetFloat("EffectVol", data.Effect);
        _Effect.value =ValueChanged_AudioMixer_To_UI(data.Effect);
        _Effect.onValueChanged.AddListener(ChangeEffect);


        _SettingHide = Tool.GetUIComponent<Button>(_RootUI, "SettingHide");
        _SettingHide.onClick.AddListener(RootClick);
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
}