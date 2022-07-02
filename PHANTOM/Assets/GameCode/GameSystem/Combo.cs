using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "ComboData", fileName = "new ComboData")]
public class Combo : ScriptableObject
{
    // TODO: make it reactive?
    [SerializeField]
    private float refreshTime = 1;
    private float sinceLastRefresh;
    public ReactiveProperty<int> counter;

    // FIXME: I don't know why ReadOnlyReactiveProperty always give me `null`
    //   so `ShowCombo` can't subscribe to it.
    // public ReadOnlyReactiveProperty<int> counter { get; private set; }

    public void Reset()
    {
        Debug.Log("Combo reset");
        this.sinceLastRefresh = 0;
        this.counter = new ReactiveProperty<int>();
        // this.counter = this._counter.ToReadOnlyReactiveProperty();
    }

    public void Add(int delta)
    {
        Debug.Assert(delta >= 0);
        if (this.counter.Value == 0)
        {
            this.sinceLastRefresh = 0;
        }
        this.counter.Value += delta;
    }

    public void Tick(float delta)
    {
        this.sinceLastRefresh += delta;
        if (this.sinceLastRefresh > this.refreshTime)
        {
            this.sinceLastRefresh = 0;
            this.counter.Value = 0;
        }
    }

    private void OnDisable()
    {
        this.Reset();
    }
}

