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
    private IDisposable playAnimationDisposable;
    public event Action OnPlayAnimationEnd; 
    [ContextMenu("Play")]
    public void Play()
    {
        var interval = totalSeconds / playlist.Count;
        playAnimationDisposable = Observable.Interval(TimeSpan.FromSeconds(interval)).Subscribe(ChangeSprite);
    }

    private void ChangeSprite(long value)
    {
        Debug.Log(value);
        if (playlist.Count <= value)
        {
            PlayAnimationEnd();
        }
        else
        {
            image.sprite = playlist[(int)value];
        }
    }

    private void PlayAnimationEnd()
    {
        playAnimationDisposable?.Dispose();
        OnPlayAnimationEnd?.Invoke();
    }
}
