using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioClip CurrentBGM;

    [SerializeField] private AudioClip StartBGM;
    [SerializeField] private List<AudioClip> LoopBGMs;

    [SerializeField] private AudioClip SelectedBGM;
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
                bgmSource.outputAudioMixerGroup = GameResource.BGMGroup;
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
    public void PlayMusicSelectMusic()
    {
        BGMSource.clip = SelectedBGM;
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
        checkLooptimer?.Dispose();
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
            source.outputAudioMixerGroup = GameResource.BGVGroup;
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

    private IDisposable checkLooptimer;

    private int loopIndex;

    public void PlayMusicAndLoop()
    {
        loopIndex = 0;
        PlayMusic(StartBGM);
        checkLooptimer = Observable.Interval(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => CheckLoop());
    }

    private void CheckLoop()
    {
        if (!BGMSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    private void PlayNextMusic()
    {
        if (LoopBGMs.Count <= 0)
        {
            checkLooptimer?.Dispose();
            return;
        }

        
        if (loopIndex >= LoopBGMs.Count)
        {
            loopIndex=0;
        }
        PlayMusic(LoopBGMs[loopIndex++]);
    }
}