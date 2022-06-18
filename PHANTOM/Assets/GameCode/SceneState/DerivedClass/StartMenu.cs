using UnityEngine;

public class StartMenu :MonoBehaviour
{
    [SerializeField] private PlayAnimation playAnimation;
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
        mainMenuUI = new MainMenuUI(playAnimation.Play);
        settingUI = new SettingUI();
        playAnimation.OnPlayAnimationEnd += ()=>Main.LoadSceneMode("Prologue");
    }
}