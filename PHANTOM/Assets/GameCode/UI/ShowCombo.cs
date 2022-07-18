using UnityEngine;
using UniRx;

public class ShowCombo : MonoBehaviour
{
    [SerializeField]
    private Combo combo;
    [SerializeField]
    private GameObject digitPrefab;

    private bool registerd;

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
        combo.counter
            .Subscribe(setComboValue)
            .AddTo(this);
        this.registerd = true;
    }

    private void setComboValue(int x)
    {
        var children = GetComponentsInChildren<ComboDigit>();
        var expectedDigitCount = this.countDigits(x);
        if (expectedDigitCount > children.Length)
        {
            for (int i = 0; i < expectedDigitCount - children.Length; i++)
            {
                var go = Instantiate(this.digitPrefab, transform);
            }
            children = GetComponentsInChildren<ComboDigit>();
        }
        else if (expectedDigitCount < children.Length)
        {
            for (int i = 0; i < children.Length - expectedDigitCount; i++)
            {
                Destroy(children[i].gameObject);
            }
            children = GetComponentsInChildren<ComboDigit>();
        }
        for (int i = children.Length - 1; i >= 0; i--)
        {
            children[i].Value = x % 10;
            x /= 10;
        }
        Debug.Log($"Combo: {x}");
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
}
