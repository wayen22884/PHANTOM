using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
public enum FSMTransition
{
    Null = 0,
    Go_Idle,
    Go_Chase,
    Go_Attack,
    Go_Dead,
}


public enum FSMStateID
{
    Null = 0,
    IdleState,
    MoveToState,
    ChaseState,
    AttackState,
    DeadState,
}

public abstract class FSMState 
{
    protected Dictionary<FSMTransition, FSMState> _FSMMap;
    protected FSMSystem _FSMSystem;
    protected float TimeCheckInterval;
    public FSMState(FSMSystem system) 
    {
        _FSMSystem = system;
        _FSMMap = new Dictionary<FSMTransition, FSMState>();
    }

    public FSMStateID ID { get; protected set; }
    public void AddFSMTransition(FSMTransition transition,FSMState FSMState)
    {
        _FSMMap.Add(transition,FSMState);
    }
    public void DeleteFSMTransition(FSMTransition transition)
    {
        if (_FSMMap.ContainsKey(transition)==false) 
            Debug.LogError($"there is no {transition} in FSMMap");
        else _FSMMap.Remove(transition);
    }

    public FSMState TranslateCheck(FSMTransition transition)
    {
        if (_FSMMap.ContainsKey(transition)==false)
        { Debug.LogError($"there is no {transition} in FSMMap");return null;}

        return _FSMMap[transition];
    }
    float TimeCheckclock = 0f;
    protected bool IsUpdateTime
    {
        get
        {
            if (TimeCheckclock < TimeCheckInterval) { TimeCheckclock += Time.deltaTime; return false; }
            else { TimeCheckclock = 0; return true; }
        }
    }

    public virtual void CheckCondition(AIData AIData)
    {

    }
    public virtual void Do(AIData AIData)
    {

    }
    public virtual void DoBeforeEnter(AIData AIData)
    {
    }
    public virtual void DoBeforeLeave()
    {

    }
}
public class FSMIdleState : FSMState
{
    public FSMIdleState(FSMSystem system):base(system)
    {
        ID = FSMStateID.IdleState;
        TimeCheckInterval = 0.17f;
    }
    public override void CheckCondition(AIData AIData)
    {
        if (!IsUpdateTime) return; 
        if (AIData.player.Death) return;
        if (AIData.EnemyPositionX > 8.5f) { _FSMSystem.Translate(FSMTransition.Go_Chase);return; }
        if (AIData.Distance < AIData.ChaseDistance) _FSMSystem.Translate(FSMTransition.Go_Attack);
        else _FSMSystem.Translate(FSMTransition.Go_Chase);

    }
    public override void Do(AIData AIData)
    {
        AIData.Character.ChangeAnimationState("idle");
    }
}
public class FSMChaseState : FSMState
{
    public FSMChaseState(FSMSystem system) : base(system)
    {
        ID = FSMStateID.ChaseState;
        TimeCheckInterval = 0.17f;

    }
    public override void CheckCondition(AIData AIData)
    {
        if (!IsUpdateTime) return;
        if (AIData.player.Death) { _FSMSystem.Translate(FSMTransition.Go_Idle);return; }
        if (AIData.EnemyPositionX > 8.5f) return;
        if (AIData.Distance < AIData.AttackDistance) _FSMSystem.Translate(FSMTransition.Go_Attack);
    }
    public override void Do(AIData AIData)
    {
        float vector = AIData.VectorDistance;
        if (AIData.Attr.SetFace(vector)) AIData.Transform.DOScaleX(-AIData.Transform.localScale.x, 0f);
        AIData.Character.ChangeAnimationState("walk");
        AIData.Transform.DOMoveX(AIData.Transform.position.x + 0.1f * AIData.Attr.MoveVelocity * Time.unscaledDeltaTime, 0f);
    }
}
public class FSMAttackState : FSMState
{
    public FSMAttackState(FSMSystem system) : base(system)
    {
        ID = FSMStateID.AttackState;
        TimeCheckInterval = 0.17f;

    }
    public override void DoBeforeEnter(AIData AIData)
    {
        AIData.Character.ChangeAnimationState("idle");
    }

    public override void CheckCondition(AIData AIData)
    {
        if (!IsUpdateTime) return;
        if (AIData.player.Death) { _FSMSystem.Translate(FSMTransition.Go_Idle);return; }
        if (AIData.EnemyPositionX > 8.5f) { _FSMSystem.Translate(FSMTransition.Go_Chase); return; }
        if (AIData.Distance > AIData.ChaseDistance) _FSMSystem.Translate(FSMTransition.Go_Chase);
    }
    public void SetAttackInterval(float value)
    {
        AttackInterval = value;
    }
    protected float AttackInterval = 1f;
    protected float AttackClock = 0f;
    public override void Do(AIData AIData)
    {
        float vector = AIData.VectorDistance;
        if (AIData.Attr.SetFace(vector)) AIData.Transform.DOScaleX(-AIData.Transform.localScale.x, 0f);
        if (AttackClock < AttackInterval) { AttackClock += Time.deltaTime; return; }
        else AttackClock = 0f;

        AIData.Character.Attack();
    }
}
public class FSMDeadState : FSMState
{
    public FSMDeadState(FSMSystem system) : base(system)
    {
        ID = FSMStateID.DeadState;
        TimeCheckInterval = 1f;
    }
}
