using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StaffPageUI : IUserInterface
{
    private Button _SettingHide;
    private Button _SettingOpen;
    private GameObject _BackGround;
    private List<GameObject> pages;
    public override void Initialize()
    {
        _RootUI = Tool.FindChildGameObject(GameResource.Canvas, "CreditsAllPages");

        GameObject go = Tool.FindChildGameObject(GameResource.Canvas, "MainSMenuSatus");
        _SettingOpen = Tool.GetUIComponent<Button>(go, "StaffButton");
        _SettingOpen.onClick.AddListener(RootClick);


        _SettingHide = Tool.GetUIComponent<Button>(_RootUI, "CloseButton");
        _SettingHide.onClick.AddListener(RootClick);

        _BackGround = Tool.FindChildGameObject(_RootUI, "BackGround");

        pages = new List<GameObject>();
        pages.Add(Tool.FindChildGameObject(_RootUI, "CreditsPage1"));
        pages.Add(Tool.FindChildGameObject(_RootUI, "CreditsPage2"));
        pages.Add(Tool.FindChildGameObject(_RootUI, "CreditsPage3"));

        UnityAction<BaseEventData> click = new UnityAction<BaseEventData>(MyClick);
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerClick;
        myclick.callback.AddListener(click);

        EventTrigger trigger = _BackGround.AddComponent<EventTrigger>();
        trigger.triggers.Add(myclick);
    }
    int count = 0;
    public void MyClick(BaseEventData data)
    {
        if (count < pages.Count - 1)
        {
            count++;
            pages[count - 1].SetActive(false);
            pages[count].SetActive(true);
        }
        else
        {
            RootClick();
        }
    }
    public override void RootClick()
    {
        base.RootClick();
        if (_bActive)
        {
            count = 0;
            pages[count].SetActive(true);
        }
        else pages[count].SetActive(false);

    }
}