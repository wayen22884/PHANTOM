using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ShowCredit : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private CreditData[] credits;
    [SerializeField]
    private GameObject[] members;
    [Header("UI")]
    [SerializeField]
    private Text nameLabel;
    [SerializeField]
    private Text jobPosition;
    [SerializeField]
    private Text sentence;

    private ReactiveProperty<CreditData> selected = new ReactiveProperty<CreditData>();

    // Start is called before the first frame update
    void Start()
    {
        this.selected
            .Where(x => x != null)
            .Subscribe(this.showCredit)
            .AddTo(this);
        for (int i = 0; i < members.Length; i++)
        {
            this.members[i]
                .OnMouseOverAsObservable()
                .Subscribe(this.subscribe(i))
                .AddTo(this);
        }
    }

    // TODO: Use a better approch to subscribe this event
    private System.Action<Unit> subscribe(int idx)
    {
        return (_) =>
        {
            this.showCredit(this.credits[idx]);
        };
    }

    private void showCredit(CreditData credit)
    {
        this.nameLabel.text = credit.Name;
        this.jobPosition.text = credit.JobPosition;
        this.sentence.text = credit.Sentence;
    }
}
