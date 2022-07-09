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
    public GameObject GameObject => Transform.gameObject;
    public Transform Transform { get; private set; }
    public Transform AttackPoint { get; private set; }
    public Animator Animator { get; private set; }

    public event Action<Transform, string, bool> DoAnimation;
    public event Action<int> ShieldAnimationCallBack;
    public event Action<Transform> BeAttackVFXAnimationCallBack;
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
        AttackPoint = Tool.FindChildGameObject(player, "AttackPoint").transform;
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
        DoAnimation?.Invoke(Transform, state, Attr.FaceRight);
        _nowState = state;
    }

    public bool ReturnIsRight => Attr.FaceRight;

    AnimatorUpdateMode originUpdateMode;

    public void ClickPause(bool pause)
    {
        if (pause)
        {
            Transform.DOPause();
            originUpdateMode = Animator.updateMode;
            Animator.updateMode = AnimatorUpdateMode.Normal;
        }
        else
        {
            Transform.DOPlay();
            Animator.updateMode = originUpdateMode;
        }

        DoPause(pause);
    }

    protected virtual void DoPause(bool pause)
    {
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

    public virtual void InJuryedAction()
    {
        Debug.Log($"{nameof(InJuryedAction)} is not override.");
    }

    public virtual void Update()
    {
    }

    public void ChangeShield(int i)
    {
        ShieldAnimationCallBack?.Invoke(i);
    }
}