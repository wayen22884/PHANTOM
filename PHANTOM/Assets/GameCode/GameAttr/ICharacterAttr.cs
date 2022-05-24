using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  ICharacterAttr
{
    protected IBaseAttr _baseAttr;
    IAttrStrategy _attrStrategy;
    int shield=3;
    public void SetNoDamage(int value)
    {
        shield = value;
    }
    public void SetBaseAttr(IBaseAttr BaseAttr)
    {
        _baseAttr = BaseAttr;
    }
    public IBaseAttr GetBaseAttr()
    {
        return _baseAttr;
    }
    public void SetAttrStrategy(IAttrStrategy AttrStrategy)
    {
        _attrStrategy = AttrStrategy;
    }
    public bool FaceRight=>_baseAttr.FaceRight;

    public float MoveVelocity
    {
        get {return _baseAttr.FaceRight ? MoveSpeed : -MoveSpeed; }
    }
    public bool SetFace(float move)
    {
        var face = _baseAttr.SetFace(move);
        if(face) Debug.Log(face);
        return face;
    }
    //取得現在生命值
    public int GetNowHP()
    {
        return _baseAttr.HP;
    }

    public int DamageCount(ICharacterAttr Target)
    {
        //若傷害公式變換則在此運用
        int damage = _attrStrategy.GetDamageValue(_baseAttr, Target._baseAttr);
        return damage;
    }
    /// <summary>
    /// 直接給正數傷害，給負數會歸0
    /// </summary>
    /// <param name="damage"></param>
    public void GetInjuryed(int damage)
    {
        if (shield>0)
        {
            shield--;
            return;
        }

        InJuryedAction();
        if (damage < 0) damage = 0;
        _baseAttr.SetHP(-damage);
        if (_baseAttr.HP==0) Dead();
    }
    protected float MoveSpeed { get => _baseAttr.MoveSpeed ; }
    public abstract void Dead();
    protected abstract void InJuryedAction();
    public abstract void ReSet();
}

