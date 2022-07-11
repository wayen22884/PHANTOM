using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioClip CurrentBGM;

    [SerializeField] public AudioClip StartBGM;
    [SerializeField] public List<AudioClip> LoopBGMs;

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
    private AudioSource LoopBGVSource
    {
        get
        {
            if (!loopBGVSource)
            {
                loopBGVSource = gameObject.AddComponent<AudioSource>();
                loopBGVSource.outputAudioMixerGroup = GameResource.BGVGroup;
            }

            return loopBGVSource;
        }
    }

    private AudioSource bgmSource;
    private AudioSource loopBGVSource;

    private Queue<AudioSourceContainer> bgvSources=new Queue<AudioSourceContainer>();

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
        BGMcheckLooptimer?.Dispose();
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

    private IDisposable BGMcheckLooptimer;
    private IDisposable BGVcheckLooptimer;

    private int loopIndex;

    public void PlayMusicAndLoop()
    {
        loopIndex = 0;
        PlayMusic(StartBGM);
        BGMcheckLooptimer?.Dispose();
        BGMcheckLooptimer = Observable.Interval(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => CheckBGMLoop());
    }

    private void CheckBGMLoop()
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
            BGMcheckLooptimer?.Dispose();
            return;
        }

        
        if (loopIndex >= LoopBGMs.Count)
        {
            loopIndex=0;
        }
        PlayMusic(LoopBGMs[loopIndex++]);
    }

    public void PlayLoopBGV(AudioClip duckWalk)
    {
        LoopBGVSource.loop = true;
        LoopBGVSource.clip=duckWalk;
        LoopBGVSource.Play();
    }

    public void StopLooopBGV()
    {
        LoopBGVSource.Stop();
    }
}