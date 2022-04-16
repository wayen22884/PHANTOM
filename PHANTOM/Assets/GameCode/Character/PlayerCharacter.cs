using UnityEngine;
using DG.Tweening;
using UniRx;
using System;

public class PlayerCharacter : ICharacter
{
    public PlayerCharacter() : base(CharacterID.Player)
    {
    }

    PlayerAttr playerAttr;
    private CharacterController controller;

    public void SetCharacterAttr(PlayerAttr characterAttr)
    {
        playerAttr = characterAttr;
    }

    public override ICharacterAttr Attr => playerAttr;

    public override void StartInput()
    {
        controller=gameObject.GetComponent<CharacterController>();
        controller.Initialize(Attack);
        controller.StartInput();
    }

    public override void Attack()
    {
        Debug.Log("Attack");
        AttackAction();
    }

    private void AttackAction()
    {
        DamageData damageData = new DamageData(Target.enemy,this, _transform.position);

        var attackTrigger = AllSourcePool.UseAttackTrigger();
        attackTrigger.Set(damageData);
    }

    public override void Attack(ICharacterAttr Target)
    {
        int damage = playerAttr.DamageCount(Target);
        Target.GetInjuryed(damage);
    }


    #region Update

    private void InputCheck()
    {
        if (pause) return;
        //進行動畫中不可輸入
        if (playAnimation) return;
        CheckFace();
        if (CheckDash()) return;
        CheckWalk();
        //CheckAttack();
    }

    bool playAnimation;

    void CheckFace()
    {
        if (playerAttr.SetFace(Input.GetAxis("Horizontal"))) _transform.DOScaleX(-_transform.localScale.x, 0f);
    }

    bool CheckDash()
    {
        ColdDash();
        if (Input.GetButtonDown("Tumbling") && Dash < 2)
        {
            Dash++;
            ChangeAnimationState("dash");
            playAnimation = true;
            Attr.SetNoDamage(true);
            float PostionNext = _transform.position.x + playerAttr.DashMove;
            if (Mathf.Abs(PostionNext) <= 8.7f)
            {
                _transform.DOMoveX(PostionNext, 0.4f).OnComplete(StopAttack)
                    .SetEase(Ease.InOutExpo);
            }
            else
            {
                PostionNext = (playerAttr.DashMove / Mathf.Abs(playerAttr.DashMove)) * 8.7f;
                float _time = Mathf.Abs((PostionNext - _transform.position.x) / playerAttr.DashMove) * 0.4f;
                _transform.DOMoveX(PostionNext, _time).OnComplete(StopAttack)
                    .SetEase(Ease.InOutExpo);
            }

            return true;
        }

        return false;
    }

    readonly float DashColdDownInterval = 1f;
    float DashColdDownClock = 0f;
    int Dash = 0;

    void ColdDash()
    {
        if (Dash > 0)
        {
            if (Tool.IsUpdateTime(ref DashColdDownClock, DashColdDownInterval))
            {
                Dash--;
            }
        }
    }

    void CheckWalk()
    {
        float AbsMove = Mathf.Abs(Input.GetAxis("Horizontal"));
        ChangeAnimationState(AbsMove != 0 ? "walk" : "idle");
        if (AbsMove != 0f)
        {
            float PositionNext = _transform.position.x + AbsMove * playerAttr.MoveVelocity * Time.unscaledDeltaTime;

            if (Mathf.Abs(PositionNext) <= 8.5f)
            {
                _transform.DOMoveX(PositionNext, 0f);
            }
            else
            {
                if (Mathf.Abs(PositionNext) < Mathf.Abs(_transform.position.x)) _transform.DOMoveX(PositionNext, 0f);
            }
        }
    }

    bool _inAttackInterval = false;
    float attackClock = 0;
    readonly float attackInterval = 0.33f;

    bool _transformation;
    public bool Transformation => _transformation;

    public void ChangeValue(ValueEventArgs args)
    {
        if (args.Type == ValueType.SP)
        {
            playerAttr.GetBaseAttr().SetSP(args.Value);
            float value = (float) Attr.GetBaseAttr().SP / Attr.GetBaseAttr().MaxSP;
            //GameSystems.Instance.UpdateUI(args.Type, value);
        }
        else if (args.Type == ValueType.EP)
        {
            playerAttr.GetBaseAttr().SetEP(args.Value);
            float value = (float) Attr.GetBaseAttr().EP / Attr.GetBaseAttr().MaxEP;
            //GameSystems.Instance.UpdateUI(args.Type, value);
        }
        else if (args.Type == ValueType.Score)
        {
            //GameSystems.Instance.UpdateScore(args.Value);
        }
        else if (args.Type == ValueType.HP)
        {
            int Recover = Mathf.RoundToInt(playerAttr.GetBaseAttr().MaxHP * args.Value / 100);
            playerAttr.GetBaseAttr().SetHP(Recover);
        }
        else
        {
            Debug.LogError("there is no ValueType");
            return;
        }
    }

    IDisposable BunkerRecoverHp = null;
    float RecoverInterval = 1f / 10f;

    void StopAttack()
    {
        playAnimation = false;
        Attr.SetNoDamage(false);
    }

    #endregion

    public override void ReSet()
    {
        throw new System.NotImplementedException();
    }

    public override void Dead()
    {
        if (_dead) return;
        base.Dead();
        _dead = true;
        ChangeAnimationState("die");
    }
}