using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "ComboData", fileName = "new ComboData")]
public class Combo : ScriptableObject
{
    // TODO: make it reactive?
    [SerializeField]
    private float timeToRefresh = 1;
    private float sinceLastHit;
    public ReactiveProperty<int> counter;

    // FIXME: I don't know why ReadOnlyReactiveProperty always give me `null`
    //   so `ShowCombo` can't subscribe to it.
    // public ReadOnlyReactiveProperty<int> counter { get; private set; }

    public void Reset()
    {
        Debug.Log("Combo reset");
        this.refreshTimer();
        this.counter = new ReactiveProperty<int>();
        // this.counter = this._counter.ToReadOnlyReactiveProperty();
    }

    public void Add(int delta)
    {
        // Debug.Assert(delta >= 0);
        this.refreshTimer();
        this.counter.Value += delta;
    }

    public void Tick(float delta)
    {
        if (this.counter.Value <= 0) return;
        this.sinceLastHit += delta;
        if (this.sinceLastHit > this.timeToRefresh)
        {
            this.refreshTimer();
            this.counter.Value = 0;
        }
    }

    private void OnDisable()
    {
        this.Reset();
    }

    private void refreshTimer()
    {
        this.sinceLastHit = 0;
    }
}
