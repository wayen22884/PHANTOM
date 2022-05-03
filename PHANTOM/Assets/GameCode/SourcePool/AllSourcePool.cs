using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
public static class AllSourcePool 
{
    public static PlayerCharacter PlayerCharacter { get; private set; }
    public static void SetPlayer(PlayerCharacter  playerCharacter)
    {
        PlayerCharacter = playerCharacter;
    }

    private static IDisposable updateDisposable;
    
    static EnemySourcePool enemy1;
    static EnemySourcePool rifles;
    static EnemySourcePool shootGuns;

    public static List<ICharacter>  GetAliveEnemyListCopy()
    {
        List<ICharacter> Result = new List<ICharacter>();
        enemy1.GetAliveCopy().ForEach(i => Result.Add(i));
        rifles?.GetAliveCopy().ForEach(i => Result.Add(i));
        shootGuns?.GetAliveCopy().ForEach(i => Result.Add(i));
        return Result;
    }
    public static ICharacter UseNewEnemy(CharacterID enemy)
    {
        if (enemy == CharacterID.Enemy) return enemy1.Use();
        else { Debug.LogError("not exist Enemy");return null; }
    }
    public static void AddToDeadList(ICharacter enemyCharacter,CharacterID enemy)
    {
        if (enemy == CharacterID.Enemy) enemy1.AddToDeads(enemyCharacter);
        else if(enemy == CharacterID.Rifle) rifles.AddToDeads(enemyCharacter);
        else if(enemy == CharacterID.ShootGun) shootGuns.AddToDeads(enemyCharacter);
        else Debug.LogError("not exist Enemy");
    }
    public static void AliveEnemyRemove(ICharacter enemyCharacter,CharacterID enemy)
    {
        if (enemy == CharacterID.Enemy) enemy1.RemoveAliveObject(enemyCharacter);
        else if (enemy == CharacterID.Rifle) rifles.RemoveAliveObject(enemyCharacter);
        else if (enemy == CharacterID.ShootGun) shootGuns.RemoveAliveObject(enemyCharacter);
        else Debug.LogError("not exist Enemy");
    }




    private static FloatingSourcePool floatingbar;
    public static void AddToNotUsingfloatingBarList(FloatingBar floatingBar)
    {
        floatingbar.AddToDeads(floatingBar);
    }
    public static FloatingBar UseFloatingBar()
    {
        return floatingbar.Use();
    }
    public static FloatingBar UseLastFloatingBar()
    {
        return floatingbar.UseLast();
    }
    public static void RecycleFloatingBar(FloatingBar floatingBar)
    {
        floatingbar.RemoveAliveObject(floatingBar);
        floatingbar.AddToDeads(floatingBar);
    }


    private static AttackTriggerPool triggerPool;
    
    
    public static void AddAttackTriggerPool(AttackTrigger attackTrigger)
    {
        triggerPool.AddToDeads(attackTrigger);
    }
    public static AttackTrigger UseAttackTrigger()
    {
        return triggerPool.Use();
    }
    public static void RecycleAttackTrigger(AttackTrigger attackTrigger)
    {
        triggerPool.RemoveAliveObject(attackTrigger);
        triggerPool.AddToDeads(attackTrigger);
    }
    
    public static void Initialize()
    {
        enemy1 = new EnemySourcePool(CharacterID.Enemy);
        enemy1.Initialize();
        // rifles = new EnemySourcePool(CharacterID.Rifle);
        // rifles.Initialize();
        // shootGuns = new EnemySourcePool(CharacterID.ShootGun);
        // shootGuns.Initialize();
         floatingbar = new FloatingSourcePool();
         floatingbar.Initialize();
         triggerPool = new AttackTriggerPool();
         triggerPool.Initialize();


         StartUpdtate();
    }

    private static void StartUpdtate()
    {
        updateDisposable = Observable.EveryUpdate().Subscribe(_ => Update());
    }

    private static void Update()
    {
        enemy1?.Update();
        rifles?.Update();
        shootGuns?.Update();
        floatingbar?.Update();
        triggerPool?.Update();
    }
    public static void Clear()
    {
        enemy1.Clear();
        rifles.Clear();
        shootGuns.Clear();
        floatingbar.Clear();
    }
}



abstract class ISourcePool<T> where T:ISourcePoolObj
{
    protected LinkedList<T> _alives;
    protected LinkedList<T> _deads;
    public void Initialize()
    {
        _alives = new LinkedList<T>();
        _deads = new LinkedList<T>();
        BeforeStart();
    }
    protected virtual void  BeforeStart()
    {

    }
    public T Use()
    {
        T Result = _deads.First!= null ? _deads.First.Value : Create();
        _deads.RemoveFirst();
        _alives.AddLast(Result);
        BrforeUse(Result);
        return Result;
    }
    public T UseLast()
    {
        T Result = _deads.Last != null ? _deads.Last.Value : Create();
        _deads.RemoveLast();
        _alives.AddLast(Result);
        BrforeUse(Result);
        return Result;
    }
    public void AddToDeads(T obj)
    {
        _deads.AddLast(obj);
    }
    public List<T> GetAliveCopy()
    {
        List<T> Result = new List<T>();
        foreach (var obj in _alives)
        {
            Result.Add(obj);
        }
        return Result;
    }
    public void RemoveAliveObject(T obj)
    {
        _alives.Remove(obj);
    }
    public void Update()
    {
        List<T> Copy = new List<T>();
        foreach (var obj in _alives)
        {
            Copy.Add(obj);
        }
        Copy.ForEach(i => i.Update());
    }


    protected abstract T Create();
    protected abstract void BrforeUse(T obj);
    public void Clear()
    {
        _alives.Clear();
        _deads.Clear();
    }
}
class EnemySourcePool:ISourcePool<ICharacter>
{
    protected CharacterID _type;
    public EnemySourcePool(CharacterID EnemyType) { _type = EnemyType; }

    protected override void BrforeUse(ICharacter obj)
    {
        obj.ReSet();
        obj.gameObject.SetActive(true);
        Factory.SetBloodBar(obj.gameObject, obj.Attr.GetBaseAttr());
    }

    protected override ICharacter Create() { return Factory.CreateEnemy(_type); }
}
class FloatingSourcePool : ISourcePool<FloatingBar>
{
    protected override void BeforeStart()
    {
        for (int i = 0; i < 10; i++) Create();
    }
    protected override void BrforeUse(FloatingBar obj)
    {
    }
    protected override FloatingBar Create()
    {
        return Factory.CreatFloatingBar();
    }
}

class AttackTriggerPool: ISourcePool<AttackTrigger>
{
    protected override AttackTrigger Create()
    {
        return Factory.CreateAttackTrigger();
    }

    protected override void BrforeUse(AttackTrigger obj)
    {
    }
}
interface ISourcePoolObj
{
    void Update();
}