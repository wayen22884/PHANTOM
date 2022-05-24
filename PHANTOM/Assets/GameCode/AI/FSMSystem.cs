using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    public FSMSystem(AIData AIData) 
    {
        this.AIData = AIData;
    }
    private FSMStateID stateID=FSMStateID.Null;
    private FSMState currentState;
    private Dictionary<FSMTransition, FSMState> FSMSystemMap= new Dictionary<FSMTransition, FSMState>();
    private AIData AIData;

    public virtual void Initialize()
    {
        var idleState = new FSMIdleState(this);
        currentState = idleState;
        stateID = FSMStateID.IdleState;

        var attackState= new FSMAttackState(this);
        var chaseState = new FSMChaseState(this);

        idleState.AddFSMTransition(FSMTransition.Go_Attack,attackState);
        idleState.AddFSMTransition(FSMTransition.Go_Chase,chaseState);

        attackState.AddFSMTransition(FSMTransition.Go_Chase,chaseState);
        attackState.AddFSMTransition(FSMTransition.Go_Idle,idleState);

        chaseState.AddFSMTransition(FSMTransition.Go_Attack,attackState);
        chaseState.AddFSMTransition(FSMTransition.Go_Idle,idleState);

        var deadState = new FSMDeadState(this);
        var BeAttackState = new BeAttackState(this);
        BeAttackState.AddFSMTransition(FSMTransition.Go_Idle,idleState);
        AddFSMTransition(FSMTransition.Go_Dead,deadState);
        AddFSMTransition(FSMTransition.Go_BeAttack,BeAttackState);
    }

    public void AddFSMTransition(FSMTransition transition, FSMState FSMState)
    {
        FSMSystemMap.Add(transition, FSMState);
    }
    public void DeleteFSMTransition(FSMTransition transition)
    {
        if (FSMSystemMap.ContainsKey(transition) == false)
            Debug.LogError($"there is no {transition} in FSMMap");
        else FSMSystemMap.Remove(transition);
    }


    public void GlobalTranslate(FSMTransition transition)
    {
        if (FSMSystemMap.ContainsKey(transition)) ChangeState(FSMSystemMap[transition]);
        else Debug.LogError($"there is no {transition} in FSMGolbalMap");
    }
    public void Translate(FSMTransition transition)
    {
        FSMState nextState= currentState.TranslateCheck(transition);
        if (nextState == null) return;
        ChangeState(nextState);
    }
    void ChangeState(FSMState NextState)
    {
        Debug.Log(NextState.ID);
        currentState.DoBeforeLeave();
        currentState = NextState;
        stateID = NextState.ID;
        currentState.DoBeforeEnter(AIData);
    }
    public void Update()
    {
        currentState.CheckCondition(AIData);
        currentState.Do(AIData);
    }
}
