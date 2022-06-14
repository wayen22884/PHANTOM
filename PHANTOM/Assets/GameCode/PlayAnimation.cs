using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimation : MonoBehaviour
{
    
    [SerializeField] private Image image;

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
        Debug.Log(value);
        if (playlist.Count <= value)
        if (playlist.Count>value)
        {
            PlayAnimationEnd();
            image.sprite = playlist[(int)value];
        }
        else
        {
            image.sprite = playlist[(int)value];
            playAnimationDisposable?.Dispose();
            OnPlayAnimationEnd?.Invoke();
        }
    {
        if (frame>=value)
        {
            var color = image.color;
            color.a= colorInterval*value;
            image.color = color;            
        }
        else
        {
            ShowSpriteDisposable?.Dispose();
            image.GetComponent<Button>().interactable = true;
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
