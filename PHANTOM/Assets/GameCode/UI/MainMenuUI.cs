using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : IUserInterface
{
    private Button _start;
    private Button _exit;
    private Action onClickStart;
    public MainMenuUI(Action onClickStart)
    {
        this.onClickStart = onClickStart;
        Initialize();
    }
    public override void Initialize()
    {
        _bActive = true;
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "MainSMenuSatus");

        _start = Tool.GetUIComponent<Button>(_RootUI, "StartBattleSceneButton");
        _start.onClick.AddListener(() =>
        {
            MusicSystem.Instance.PlayBGV(GameResource.Button_Yes);
            RootClick();
            onClickStart?.Invoke();
        });

        _exit = Tool.GetUIComponent<Button>(_RootUI, "ExitButtion");
        _exit.onClick.AddListener(() => MusicSystem.Instance.PlayBGV(GameResource.Button_Yes));
        _exit.onClick.AddListener(Application.Quit);

        Tool.GetUIComponent<Button>(_RootUI, "CreditButton").onClick.AddListener(() =>
        {
            Main.LoadSceneMode("Credit");
        });
        //Tool.GetUIComponent<Button>(_RootUI, "ControlButton");
    }


}