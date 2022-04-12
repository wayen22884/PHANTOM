using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    public FSMSystem(AIData AIData) 
    {
        m_currentID = 0;
        m_currentState = null;
        m_AIData = AIData;
        _FSMSystemMap = new Dictionary<FSMTransition, FSMState>();
    }
    private FSMStateID m_currentID;
    private FSMState m_currentState;
    private Dictionary<FSMTransition, FSMState> _FSMSystemMap;
    private AIData m_AIData;

    public virtual void Initialize()
    {
        FSMIdleState idleState = new FSMIdleState(this);
        m_currentState = idleState;
        m_currentID = FSMStateID.IdleState;

        FSMAttackState attackState=m_AIData.Character.ID==CharacterID.Rifle?new FSMRifleAttackState(this): new FSMAttackState(this);
        FSMChaseState chaseState = new FSMChaseState(this);

        idleState.AddFSMTransition(FSMTransition.Go_Attack,attackState);
        idleState.AddFSMTransition(FSMTransition.Go_Chase,chaseState);

        attackState.AddFSMTransition(FSMTransition.Go_Chase,chaseState);
        attackState.AddFSMTransition(FSMTransition.Go_Idle,idleState);
        if(m_AIData.Character.ID==CharacterID.Rifle) attackState.SetAttackInterval(1.5f);
        if(m_AIData.Character.ID==CharacterID.ShootGun) attackState.SetAttackInterval(2f);

        chaseState.AddFSMTransition(FSMTransition.Go_Attack,attackState);
        chaseState.AddFSMTransition(FSMTransition.Go_Idle,idleState);

        FSMDeadState deadState = new FSMDeadState(this);
        AddFSMTransition(FSMTransition.Go_Dead,deadState);
    }

    public void AddFSMTransition(FSMTransition transition, FSMState FSMState)
    {
        _FSMSystemMap.Add(transition, FSMState);
    }
    public void DeleteFSMTransition(FSMTransition transition)
    {
        if (_FSMSystemMap.ContainsKey(transition) == false)
            Debug.LogError($"there is no {transition} in FSMMap");
        else _FSMSystemMap.Remove(transition);
    }


    public void GlobalTranslate(FSMTransition transition)
    {
        if (_FSMSystemMap.ContainsKey(transition)) ChangeState(_FSMSystemMap[transition]);
        else Debug.LogError($"there is no {transition} in FSMGolbalMap");
    }
    public void Translate(FSMTransition transition)
    {
        FSMState nextState= m_currentState.TranslateCheck(transition);
        if (nextState == null) return;
        ChangeState(nextState);
    }
    void ChangeState(FSMState NextState)
    {
        m_currentState.DoBeforeLeave();
        m_currentState = NextState;
        m_currentID = NextState.ID;
        m_currentState.DoBeforeEnter(m_AIData);
    }
    public void Update()
    {
        m_currentState.CheckCondition(m_AIData);
        m_currentState.Do(m_AIData);
    }
}
