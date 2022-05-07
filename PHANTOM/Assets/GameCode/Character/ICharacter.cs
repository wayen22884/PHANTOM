using System;
using UnityEngine;
using DG.Tweening;
using UniRx;

public abstract class ICharacter : ISourcePoolObj
{
    public ICharacter(CharacterID characterID)
    {
        ID = characterID;
    }

    public abstract ICharacterAttr Attr { get; }
    public CharacterID ID { get; }
    public GameObject gameObject => Transform.gameObject;
    public Transform Transform { get; private set; }
    public Animator Animator { get; private set; }
    public bool Death { get; protected set; }

    protected Transform model;
    protected string _nowState;
    protected Subject<ValueEventArgs> _valueHandle = new Subject<ValueEventArgs>();
    public IDisposable AddValueEventHandler(ValueEventHandler handler)
    {
        return _valueHandle.Subscribe(_ => handler(_));
    }

    //設定使用模型
    public void SetGameObject(GameObject player, GameObject model)
    {
        Transform = player.transform;
        this.model = model.transform;
        Animator = this.model.GetComponent<Animator>();
        if (Animator == null) Debug.LogError("NoAnimator");
    }

    public abstract void StartInput();
    public abstract void Attack();

    public void ChangeAnimationState(string state)
    {
        if (_nowState == state) return;
        Animator.Play(state);
        _nowState = state;
    }

    public bool ReturnIsRight => Attr.FaceRight;

    protected bool pause;
    AnimatorUpdateMode forpause;

    public void Pause()
    {
        if (!pause)
        {
            Transform.DOPause();
            forpause = Animator.updateMode;
            Animator.updateMode = AnimatorUpdateMode.Normal;
        }
        else
        {
            Transform.DOPlay();
            Animator.updateMode = forpause;
        }

        pause = !pause;
    }

    protected void SetNormalScaleTime()
    {
        Animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public abstract void Attack(ICharacterAttr target);
    public abstract void ReSet();

    public virtual void Dead()
    {
        Debug.Log("Dead is not override.");
    }

    public virtual void Update()
    {
    }
}