using UnityEngine;

public abstract class ICharacterAttr
{
    private ICharacter character;
    protected IBaseAttr _baseAttr;
    IAttrStrategy _attrStrategy;
    bool _noDamage;
    public void SetNoDamage(bool value)
    {
        _noDamage = value;
    }
    public void SetShield(int value)
    {
        _baseAttr.Shield = value;
    }

    public void SetBaseAttr(IBaseAttr BaseAttr)
    {
        _baseAttr = BaseAttr;
    }
    public void SetBaseCharacter(ICharacter character)
    {
        this.character=character;
    }

    public IBaseAttr GetBaseAttr()
    {
        return _baseAttr;
    }

    public void SetAttrStrategy(IAttrStrategy AttrStrategy)
    {
        _attrStrategy = AttrStrategy;
    }

    public bool FaceRight => _baseAttr.FaceRight;

    public float MoveVelocity
    {
        get { return _baseAttr.FaceRight ? MoveSpeed : -MoveSpeed; }
    }

    public float StiffTime => _baseAttr.StiffTime;
    public bool SetFace(float move)
    {
        var face = _baseAttr.SetFace(move);
        if (face) Debug.Log(face);
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
    public void GetInjuryed(int damage,int attackType=0)
    {
        if (_noDamage) return;
        if (_baseAttr.Shield > 0)
        {
            _baseAttr.Shield--;
            character.ChangeShield(_baseAttr.Shield);
            return;
        }

        if (attackType!=0)
        {
            CountCombo(attackType);
        }
        if (damage < 0) damage = 0;
        _baseAttr.SetHP(-damage);
        if (_baseAttr.HP <= 0)
            Dead();
        else
        {
            InJuryedAction();
        }
    }

    protected virtual void CountCombo(int attackType){}

    protected float MoveSpeed
    {
        get => _baseAttr.MoveSpeed;
    }

    public abstract void Dead();
    protected abstract void InJuryedAction();
    public abstract void ReSet();
}