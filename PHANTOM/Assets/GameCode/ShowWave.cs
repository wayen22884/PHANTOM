using System;
using System.Collections;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using DG;
using DG.Tweening;
using UniRx;
using UnityEngine.UI;

public class ShowWave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Text text1;
    private Coroutine disposable;
    public float colorAdd = 0.01f;
    
    
    public void Show(int waveIndex)
    {
        text.text = $"Wave{waveIndex}";
        if (disposable!=null)
        {
            StopCoroutine(disposable);
            disposable = null;
        }
        disposable=StartCoroutine(FadeInFadeOut());
    }

    private IEnumerator FadeInFadeOut()
    {
        var color = text.color;
        color.a = 0;
        yield return null;

        while (color.a<1)
        {
            color.a += colorAdd;
            text.color = color;
            yield return null;
        }
        
        yield return new WaitForSeconds(5);
        
        while (color.a>0)
        {
            color.a -= colorAdd;
            text.color = color;
            yield return null;
        }
        
    }

    public int TestWaveindex;
    [ContextMenu("TestSpeed")]
    private void TestSpeed()
    {
        Show(TestWaveindex);
    }
}
