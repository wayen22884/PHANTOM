using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShowCombo : MonoBehaviour
{
    [SerializeField]
    private Combo combo;

    private Text comboText;
    private bool registerd;

    void Start()
    {
        this.comboText = GetComponent<Text>();
        Debug.Assert(this.combo != null);
        Debug.Assert(this.comboText != null);
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
            .Subscribe(x =>
            {
                this.comboText.text = $"{x} Combo";
                Debug.Log($"Combo: {x}");
            })
            .AddTo(this);
        this.registerd = true;
    }
}
