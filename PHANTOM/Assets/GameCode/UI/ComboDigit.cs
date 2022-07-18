using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ComboDigit : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites;
    private Image image;

    public int Value
    {
        get => this._value;
        set => this.setValue(value);
    }
    private int _value;
    private Tween digitTransition;

    void Start()
    {
        Debug.Assert(this.sprites.Count == 10, "There sohuld be exactly 10 digits");
        this.image = GetComponent<Image>();
    }

    private void setValue(int newValue)
    {
        if (newValue < 0 || newValue > 9)
        {
            Debug.LogWarning($"ComboDigit can only present one non-negative digit.\nBut got {newValue}.");
            return;
        }
        if (newValue == this._value)
        {
            return;
        }

        this.digitTransition?.Complete();
        this.digitTransition = DOTween.Sequence()
            .Append(
                transform
                    .DOScale(1.1f, 0.1f)
                    .OnComplete(() => this.image.sprite = this.sprites[newValue])
            )
            .Append(transform.DOScale(1f, 0.1f));
        this._value = newValue;
    }
}
