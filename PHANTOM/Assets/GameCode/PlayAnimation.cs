using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayAnimation : MonoBehaviour
{
    
    [SerializeField] private Image image;
    [SerializeField] private List<Image> leafImages=new List<Image>();
    [SerializeField] private List<Image> closeleafImages=new List<Image>();

    [SerializeField] private List<Sprite> playlist;

    public float totalSeconds;
    private float colorInterval;
    private IDisposable playAnimationDisposable;
    private IDisposable ShowSpriteDisposable;
    private int frame= 30;
    public event Action OnPlayAnimationEnd; 
    public event Action OnShowSpriteEnd; 
    public event Action OnClickCollider; 
    [ContextMenu("Play")]
    public void Play()
    {
        var interval = totalSeconds / playlist.Count;
        foreach (var closeleafImage in closeleafImages)
        {
            closeleafImage.gameObject.SetActive(false);
        }
        playAnimationDisposable = Observable.Interval(TimeSpan.FromSeconds(interval)).Subscribe(ChangeSprite);
    }
    [ContextMenu("ShowSprite")]
    public void ShowSprite()
    {
        var interval = totalSeconds / frame;
        colorInterval = interval;
        ShowSpriteDisposable = Observable.Interval(TimeSpan.FromSeconds(interval)).Subscribe(ChangeColor);
    }
    
    private void ChangeSprite(long value)
    {
        if (playlist.Count>value)
        {
            image.sprite = playlist[(int)value];
        }
        else
        {
            playAnimationDisposable?.Dispose();
            OnPlayAnimationEnd?.Invoke();
        }
    }

    private void ChangeColor(long value)
    {
        if (frame>=value)
        {
            var color = image.color;
            color.a= colorInterval*value;
            foreach (var leafImage in leafImages)
            {
                leafImage.color = color;
            }
            image.color = color;            
        }
        else
        {
            ShowSpriteDisposable?.Dispose();
            image.GetComponent<Button>().interactable = true;
            image.raycastTarget = true;
            OnShowSpriteEnd?.Invoke();
        }
    }

    private void PlayAnimationEnd()
    {
        playAnimationDisposable?.Dispose();
        OnPlayAnimationEnd?.Invoke();
    }

    public void OnClickSprite()
    {
        OnClickCollider?.Invoke();
    }
}
