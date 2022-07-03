using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ => Debug.Log("hello"));
        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ => Debug.Log("hello"));
        Observable.Timer(TimeSpan.FromSeconds(4f)).Subscribe(_ =>
        {
            Debug.LogWarning("timeStop");
            Time.timeScale = 0;
        });
        Observable.Interval(TimeSpan.FromSeconds(1f)).Subscribe(_ => Debug.Log($"time:{_}"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
