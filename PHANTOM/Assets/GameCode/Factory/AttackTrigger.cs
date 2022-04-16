using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public abstract class AttackTrigger : ISourcePoolObj
{
    private List<ICharacter> attackedList = new List<ICharacter>();
    public AttackTrigger(GameObject GO)
    {
        _transform = GO.transform;
    }
    DamageData Data;
    protected Transform _transform;
    protected float face;
    GameObject GO => _transform.gameObject;

    public void Update()
    {
        DoHit();
        if (ExistTooLong())
        {
            OnDisable();
        }
        else
        {
            DoMove();
        }
    }

    protected abstract void DoMove();

    void DoHit()
    {
        if (Data.EnemyList.Count == 0) return;
        var targets = FindTargets();
        if (targets.Count == 0) return;
        targets.ForEach(target => Data.Attacker.Attack(target.Attr));
        targets.ForEach(target => attackedList.Add(target));
    }

    List<ICharacter> FindTargets()
    {
        List<ICharacter> Result = new List<ICharacter>();
        foreach (var character in Data.EnemyList)
        {
            if (!attackedList.Contains(character) && IsInAttackTrigger(character))
            {
                Result.Add(character);
            }
        }
        return Result;
    }

    protected abstract bool IsInAttackTrigger(ICharacter character);

    protected abstract bool ExistTooLong();
    public void Set(DamageData damageData)
    {
        _transform.position = damageData.startPoint;
        face = damageData.FaceRight ? 1f : -1f;
        Data = damageData;
        GO.SetActive(true);
    }
    protected virtual void OnDisable()
    {
        GO.SetActive(false);
        AllSourcePool.RecycleAttackTrigger(this);
        attackedList.Clear();
        face = 0;
    }
}


public class CircleAttackeTrigger : AttackTrigger
{
    public CircleAttackeTrigger(GameObject GO) : base(GO)
    {
    }

    protected override void DoMove()
    {
    }

    private float radius=1;
    protected override bool IsInAttackTrigger(ICharacter character)
    {
        var distance= Vector3.Distance(_transform.position, character.Transform.position);
        Debug.Log(distance);
        return radius >= distance;
    }

    private float existTime;
    private float maxExistTime = 0.5f;
    protected override bool ExistTooLong()
    {
        existTime += Time.deltaTime;
        return maxExistTime <= existTime;
    }

    protected override void OnDisable()
    {
        existTime = 0;
        base.OnDisable();
    }
}


public class RectangleAttackTrigger : AttackTrigger
{
    public RectangleAttackTrigger(GameObject GO) : base(GO)
    {
    }

    protected override void DoMove()
    {
    }

    private float length = 1;
    private float width = 1;
    protected override bool IsInAttackTrigger(ICharacter character)
    {
        var selfTriangle = new RectAngle(_transform.position, new Vector2(length, width));
        var otherTriangle = new RectAngle(character.Transform.position, new Vector2(length, width));
        return Tool.GetIntersection(selfTriangle, otherTriangle);
    }

    protected override bool ExistTooLong()
    {
        return true;
    }
}

