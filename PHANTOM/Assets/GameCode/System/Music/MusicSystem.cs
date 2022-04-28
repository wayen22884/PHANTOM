using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField]private AudioClip CurrentBGM;

    public static MusicSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    private AudioSource BGMSource
    {
        get
        {
            if (!bgmSource)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
            }

            return bgmSource;
        }
    }

    private AudioSource bgmSource;

    private Queue<AudioSourceContainer> bgvSources;
    
    public void PlayMusic()
    {
        BGMSource.clip = CurrentBGM;
        BGMSource.Play();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        CurrentBGM = audioClip;
        PlayMusic();
    }
    public void StopMusic()
    {
        BGMSource.Stop();
    }

    public void PlayBGV(AudioClip audioClip)
    {
        var source = GetNotUsedBGVSource();
        source.Play(audioClip);
    }

    private AudioSourceContainer GetNotUsedBGVSource()
    {
        if (bgvSources.Count > 0)
        {
            return bgvSources.Dequeue();
        }
        else
        {
            var source = gameObject.AddComponent<AudioSource>();
            return new AudioSourceContainer(source, bgvSources);
        }
    }
    
    private class AudioSourceContainer
    {
        private AudioSource audioSource;

        private Queue<AudioSourceContainer> recycleList;

        private IDisposable checkRecycleDisposable;

        public AudioSourceContainer(AudioSource audioSource, Queue<AudioSourceContainer> list)
        {
            this.audioSource = audioSource;
            recycleList = list;
        }

        public void SetClip(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }

        public void Play()
        {
            audioSource.Play();
            checkRecycleDisposable = Observable.EveryUpdate().Subscribe((_) => CheckRecycle());
        }

        public void Play(AudioClip audioClip)
        {
            SetClip(audioClip);
            Play();
        }

        private void CheckRecycle()
        {
            if (!audioSource.isPlaying)
            {
                checkRecycleDisposable?.Dispose();
                recycleList.Enqueue(this);
            }
        }
    }
}