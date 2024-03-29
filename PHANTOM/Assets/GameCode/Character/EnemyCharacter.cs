﻿using System;
using UniRx;
using UnityEngine;

public class EnemyCharacter : ICharacter
{
    public EnemyCharacter(string name, CharacterID characterID) : base(characterID)
    {

    }
    // HACK: This should not expose, at least not in this way...
    public EnemyAttr E_Attr => this._enemyAttr;
    EnemyAttr _enemyAttr;
    private FSMSystem _FSM;
    public Combo combo;
    public void SetCharacterAttr(EnemyAttr characterAttr)
    {
        _enemyAttr = characterAttr;
    }
    public override ICharacterAttr Attr => _enemyAttr;
    private CharacterController controller;

    public override void StartInput()
    {
        controller = GameObject.GetComponent<CharacterController>();
        controller.OnChangeState = null;
        controller.ReturnIsRight = () => ReturnIsRight;
        controller.SetFace = Attr.SetFace;
        controller.attackAction = Attack;

        controller.AttackInput = () => false;
        controller.DashInput = () => false;
        controller.MoveInput = () => 0f;
        controller.JumpInput = () => false;

        controller.StartInput();
    }

    public override void Attack()
    {
        MusicSystem.Instance.PlayBGV(GameResource.Sheep_Attack);
        AttackAction();
    }
    private void AttackAction()
    {
        DamageData damageData = new DamageData(Target.player, this, AttackPoint.position);

        var attackTrigger = AllSourcePool.UseAttackTrigger();
        attackTrigger.Set(damageData);
    }
    public override void Attack(ICharacterAttr Target, int attackType)
    {
        int damage = _enemyAttr.DamageCount(Target);
        Target.GetInjuryed(damage);
    }
    public override void Update()
    {
        _FSM.Update();
    }
    public override void ReSet()
    {
        _nowState = "idle";
        _enemyAttr.ReSet();
    }

    int score
    {
        get
        {
            if (ID == CharacterID.Enemy) return 100;
            if (ID == CharacterID.Rifle) return 150;
            if (ID == CharacterID.ShootGun) return 200;
            else { Debug.LogError("socre is wrong"); return 0; }
        }
    }


    public override void Dead()
    {
        AllSourcePool.AliveEnemyRemove(this, ID);
        ChangeAnimationState("Die");
        MusicSystem.Instance.PlayBGV(GameResource.Sheep_Die);
        TimeEventCheck.AddTimeEvent(Recycle, 1.433f, TimeEventCheck.TimeScale.ScaleTime);
        if (AllSourcePool.PlayerCharacter.Transformation) return;
    }
    void Recycle()
    {
        AllSourcePool.AddToDeadList(this, ID);
        Transform.gameObject.SetActive(false);
    }
    public void SetAI()
    {
        AIData AIData = new AIData(this, this.Transform, AllSourcePool.PlayerCharacter);
        _FSM = new FSMSystem(AIData);
        AIData.SetFSMSystem(_FSM);
        _FSM.Initialize();
    }

    public override void InJuryedAction()
    {
        MusicSystem.Instance.PlayBGV(GameResource.Sheep_Hurt);
        _FSM.GlobalTranslate(FSMTransition.Go_BeAttack);
    }
}
