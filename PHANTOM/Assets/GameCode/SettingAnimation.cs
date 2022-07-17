using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class SettingAnimation : MonoBehaviour
{
    public event Action<bool> MenuStatusCallBack;
    public event Action<bool> rootUICallBack;
    public event Action<bool> Interactable;
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> sprites=new List<Sprite>();
    [SerializeField] private List<Sprite> closeSprites=new List<Sprite>();

    
    public float interval=0.5f;
    private Coroutine ShowSpriteDisposable;
    public void PlayAnimation()
    {
        //Interactable?.Invoke(false);
        image.sprite = sprites[0];
        rootUICallBack?.Invoke(true);
        if (ShowSpriteDisposable!=null)
        {
            StopCoroutine(ShowSpriteDisposable);
        }
        ShowSpriteDisposable=StartCoroutine(ChangeCSprite(MenuStatusCallBack,true));
    }

    public void Close()
    {
        //Interactable?.Invoke(false);
        image.sprite = closeSprites[0];
        MenuStatusCallBack?.Invoke(false);
        if (ShowSpriteDisposable!=null)
        {
            StopCoroutine(ShowSpriteDisposable);
        }
        ShowSpriteDisposable=StartCoroutine(ChangeCSprite(rootUICallBack,false));
    }

    private IEnumerator ChangeCSprite(Action<bool> end,bool open)
    {
        var index = 0;
        
        List<Sprite> spriteList = open ? sprites : closeSprites;

        while (index<spriteList.Count)
        {
            image.sprite = spriteList[index];
            yield return new WaitForSecondsRealtime(interval);
            index++;
        }
            //Interactable.Invoke(true);
            end?.Invoke(open);
            StopCoroutine(ShowSpriteDisposable);
    }
}
