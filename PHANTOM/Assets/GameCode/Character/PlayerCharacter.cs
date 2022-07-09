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

    public override ICharacterAttr Attr => playerAttr;
    public void SetCharacterAttr(PlayerAttr characterAttr)
    {
        playerAttr = characterAttr;
    }

    public override void StartInput()
    {
        controller = GameObject.GetComponent<CharacterController>();
        controller.OnChangeState = ChangeAnimationState;
        controller.ReturnIsRight = () => ReturnIsRight;
        controller.SetFace = move =>
        {
            var changeFace = Attr.SetFace(move);
            if (changeFace)
            {
                var localScale = Mathf.Abs(Transform.localScale.x);
                Transform.DOScaleX(move >= 0 ? localScale : -localScale, 0f);
            }

            return changeFace;
        };
        controller.attackAction = Attack;

        controller.AttackInput = () => Input.GetButtonDown("NormalAttack");
        controller.DashInput = () => Input.GetButtonDown("Dash");
        controller.MoveInput = () => Input.GetAxis("Horizontal");
        controller.JumpInput = () => Input.GetButtonDown("Jump");
        controller.StartInput();
    }

    private IDisposable detectAttack;
    private int attackTime;
    public override void Attack()
    {
        attackTime = attackTime % 3;
        attackTime++;
        detectAttack?.Dispose();
        detectAttack = Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ => attackTime = 0);
        ChangeAnimationState($"Smash_{attackTime}");
        Debug.Log($"Smash_{attackTime}");
        AttackAction();
    }

    private void AttackAction()
    {
        DamageData damageData = new DamageData(Target.enemy, this, AttackPoint.position);

        var attackTrigger = AllSourcePool.UseAttackTrigger();
        attackTrigger.Set(damageData);
    }

    public override void Attack(ICharacterAttr Target)
    {
        int damage = playerAttr.DamageCount(Target);
        Target.GetInjuryed(damage);
    }


    #region Update

    bool _transformation;
    public bool Transformation => _transformation;

    public void ChangeValue(ValueEventArgs args)
    {
        if (args.Type == ValueType.SP)
        {
            playerAttr.GetBaseAttr().SetSP(args.Value);
            float value = (float)Attr.GetBaseAttr().SP / Attr.GetBaseAttr().MaxSP;
            //GameSystems.Instance.UpdateUI(args.Type, value);
        }
        else if (args.Type == ValueType.EP)
        {
            playerAttr.GetBaseAttr().SetEP(args.Value);
            float value = (float)Attr.GetBaseAttr().EP / Attr.GetBaseAttr().MaxEP;
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

    #endregion

    public override void InJuryedAction()
    {
        Attr.SetNoDamage(true);
        ChangeAnimationState("Hurt");
        controller.StopInput(0.5f);
        Observable.Timer(TimeSpan.FromSeconds(playerAttr.NoDamageTime)).Subscribe(_ => Attr.SetNoDamage(false));
    }
    public override void ReSet()
    {
    }

    public override void Dead()
    {
        if (Death) return;
        Death = true;
        controller.StopInputDetectAndPhysicsCaculation();
        ChangeAnimationState("Die");
        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ => BattleScene.Instance.GameEnd(false));
    }

    protected override void DoPause(bool pause)
    {
        controller.DoPause(pause);
    }

    public override void Update()
    {
    }
}
