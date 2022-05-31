using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : IUserInterface
{
    private Button _start;
    private Button _exit;

    public MainMenuUI()
    {
        Initialize();
    }
    public override void Initialize()
    {
        
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "MainSMenuSatus");
        
        _start = Tool.GetUIComponent<Button>(_RootUI, "StartBattleSceneButton");
        _start.onClick.AddListener(()=>Main.LoadSceneMode("BattleScene"));
        
        _exit = Tool.GetUIComponent<Button>(_RootUI, "ExitButtion");
        _exit.onClick.AddListener(Application.Quit);
        
        //Tool.GetUIComponent<Button>(_RootUI, "StaffButton");
        //Tool.GetUIComponent<Button>(_RootUI, "ControlButton");
    }
}