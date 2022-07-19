using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class ShowCombo : MonoBehaviour
{
    [SerializeField]
    private Combo combo;
    [SerializeField]
    private GameObject digitPrefab;
    [SerializeField]
    private Transform digitRoot;
    [SerializeField]
    private Image floatingText;
    [SerializeField]
    private Sprite[] floatingTextSprites;

    private bool registerd;
    private Tween floating;
    private Coroutine updateDigit;
    private bool isUpdatingDigit;

    void Start()
    {
        Debug.Assert(this.combo != null);
        // Continously call `register` to solve the dependecy issue
        Observable.EveryUpdate()
            .Where(_ => !this.registerd)
            .Subscribe(_ => this.register())
            .AddTo(this);
    }

    private void register()
    {
        if (this.combo.counter == null)
            return;
        this.combo.counter
            .Throttle(System.TimeSpan.FromSeconds(0.1))
            .Subscribe(this.updateComboValue)
            .AddTo(this);
        this.combo.counter
            .Throttle(System.TimeSpan.FromSeconds(0.1))
            .Subscribe(this.updateFloatingText)
            .AddTo(this);
        this.registerd = true;
    }

    private void updateComboValue(int x)
    {
        this.adjustDigitCount(x);
        if (this.isUpdatingDigit)
        {
            StopCoroutine(this.updateDigit);
            this.isUpdatingDigit = false;
        }
        this.updateDigit = StartCoroutine(this.updateDigitValue(x));
        Debug.Log($"Combo: {x}");
    }

    private IEnumerator updateDigitValue(int x)
    {
        this.isUpdatingDigit = true;
        ComboDigit[] children = this.digitRoot.GetComponentsInChildren<ComboDigit>();
        for (int i = children.Length - 1; i >= 0; i--)
        {
            children[i].Value = x % 10;
            x /= 10;
            yield return new WaitForSeconds(0.05f);
        }
        this.isUpdatingDigit = false;
    }

    private void adjustDigitCount(int x)
    {
        var children = this.digitRoot.GetComponentsInChildren<ComboDigit>();
        var expectedDigitCount = this.countDigits(x);
        if (expectedDigitCount > children.Length)
        {
            for (int i = 0; i < expectedDigitCount - children.Length; i++)
            {
                var go = Instantiate(this.digitPrefab, this.digitRoot);
            }
        }
        else if (expectedDigitCount < children.Length)
        {
            for (int i = 0; i < children.Length - expectedDigitCount; i++)
            {
                Destroy(children[i].gameObject);
            }
        }
    }

    private int countDigits(int x)
    {
        if (x == 0) return 1;
        int cnt = 0;
        while (x > 0)
        {
            x /= 10;
            cnt++;
        }
        return cnt;
    }

    private void updateFloatingText(int comboValue)
    {
        // TODO: Avoid hard-coded thresholds
        int step = 10;
        int threshold = (this.floatingTextSprites.Length - 1) * step;
        for (int i = this.floatingTextSprites.Length - 1; i >= 0; i--)
        {
            if (comboValue >= threshold)
            {
                this.floatingText.sprite = this.floatingTextSprites[i];
                this.floatingText.SetNativeSize();
                break;
            }
            threshold -= step;
        }
        this.floating?.Complete();
        var jump = this.floatingText.transform
            .DOMoveY(this.floatingText.transform.position.y + 40, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        var scale = this.floatingText.transform
            .DOScale(this.floatingText.transform.localScale.x * 1.1f, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        this.floating = DOTween.Sequence()
            .Append(jump)
            .Insert(0, scale);
    }

    void OnDestroy()
    {
        this.floatingText.transform.DOKill();
    }
}
