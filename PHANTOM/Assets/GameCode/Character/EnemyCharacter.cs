using System;
using UniRx;
using UnityEngine;

public class EnemyCharacter : ICharacter
{ 
    public EnemyCharacter(string name, CharacterID characterID) : base(characterID)
    {

    }
    EnemyAttr _enemyAttr;
    private FSMSystem _FSM;
    public void SetCharacterAttr(EnemyAttr characterAttr)
    {
        _enemyAttr = characterAttr;
    }
    public override ICharacterAttr Attr => _enemyAttr;

    public override void StartInput()
    {
        throw new NotImplementedException();
    }

    public override void Attack()
    {
        Debug.Log("EnemyAttack");
    }

    public override void Attack(ICharacterAttr Target)
    {
        int damage = _enemyAttr.DamageCount(Target);
        Target.GetInjuryed(damage);
    }
    public override void Update()
    {
        if (pause) return;
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
            if (ID== CharacterID.Enemy) return 100;
            if (ID == CharacterID.Rifle) return 150;
            if (ID == CharacterID.ShootGun) return 200;
            else { Debug.LogError("socre is wrong");return 0; }
        }
    }


    public override void Dead()
    {
        AllSourcePool.AliveEnemyRemove(this, ID);
        ChangeAnimationState("die");
        TimeEventCheck.AddTimeEvent(Recycle, 0.433f, TimeEventCheck.TimeScale.ScaleTime);
        if (AllSourcePool.PlayerCharacter.Transformation) return;
        _valueHandle.OnNext(new ValueEventArgs(ValueType.SP,1));
        _valueHandle.OnNext(new ValueEventArgs(ValueType.EP,10));
        _valueHandle.OnNext(new ValueEventArgs(ValueType.Score, score));
        
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
}
