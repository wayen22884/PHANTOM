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
        controller = gameObject.GetComponent<CharacterController>();
        controller.OnChangeState = ChangeAnimationState;
        controller.ReturnIsRight = () =>ReturnIsRight;
        controller.SetFace = Attr.SetFace;
        controller.attackAction = Attack;
        controller.StartInput();
    }

    public override void Attack()
    {
        Debug.Log("Attack");
        AttackAction();
    }

    private void AttackAction()
    {
        DamageData damageData = new DamageData(Target.enemy, this, Transform.position);

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

    #endregion

    public override void ReSet()
    {
    }

    public override void Dead()
    {
        if (Death) return;
        Death = true;
        ChangeAnimationState("die");
    }
}