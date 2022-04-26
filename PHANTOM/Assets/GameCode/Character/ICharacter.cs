using System;
using UnityEngine;
using DG.Tweening;
using UniRx;

public abstract class ICharacter:ISourcePoolObj
{
    public ICharacter(CharacterID characterID)
    {
        _ID=characterID;
    }
    public abstract ICharacterAttr Attr { get; }
    protected CharacterID _ID;
    public CharacterID ID => _ID;
    protected Transform _transform;
    protected Transform model;
    protected Animator _animator;
    protected string _nowState;
    protected bool _dead;
    public bool Death => _dead;
    protected Subject<ValueEventArgs> _valueHandle=new Subject<ValueEventArgs>();
    public IDisposable AddValueEventHandler(ValueEventHandler handler)
    {
        return _valueHandle.Subscribe(_ => handler(_));
    }
    //設定使用模型
    public void SetGameObject(GameObject player,GameObject model)
    {
        _transform = player.transform;
        this.model = model.transform;
        _animator = this.model.GetComponent<Animator>();
        if (_animator == null) Debug.LogError("NoAnimator");
        //spriteRenderer = model.transform.GetComponent<SpriteRenderer>();
        //if (spriteRenderer == null) Debug.LogError("spriteRenderer");
    }
    public GameObject gameObject => _transform.gameObject;
    public Transform Transform => _transform;
    public Animator Animator => _animator;

    public abstract void StartInput();
    public abstract void Attack();
    public void ChangeAnimationState(string state)
    {
        if (_nowState == state) return;
        _animator.Play(state);
        _nowState = state;
    }
    public void ChangeAnimationState(string state,float value)
    {
        if (Attr.SetFace(Input.GetAxis("Horizontal")))
        {
            model.DOScaleX(-model.localScale.x, 0f);
        }
        _animator.SetFloat(state,value);
        _nowState = state;
    }

    protected bool pause;
    AnimatorUpdateMode forpause;
    public void Pause()
    {
        if (!pause)
        {
            _transform.DOPause();
            forpause = Animator.updateMode;
            Animator.updateMode = AnimatorUpdateMode.Normal;
        }
        else
        {
            _transform.DOPlay();
            Animator.updateMode = forpause;
        }
        pause = !pause;
    }
    protected void SetNormalScaleTime()
    {
        _animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public abstract void Attack(ICharacterAttr target);
    public abstract void ReSet();
    public virtual void Dead() 
    {
        Debug.Log("Dead is not override.");
    }

    public virtual void Update(){}
}
