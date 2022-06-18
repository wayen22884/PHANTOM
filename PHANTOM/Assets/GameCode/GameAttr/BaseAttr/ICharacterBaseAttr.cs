using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacterBaseAttr :ScriptableObject,  IBaseAttr
{
    [SerializeField]
    protected string _attrName;
    //取得BaseAttr名字
    public string AttrName => _attrName;
    [SerializeField]
    protected string _attrType;
    //取得BaseAttr類型
    public string AttrType => _attrType;
    [SerializeField]
    protected int _maxHP;
    //取得最後加總最大生命值
    [SerializeField]
    protected int _HP;
    public int HP => _HP;
    public int MaxHP => _maxHP;


    [SerializeField]
    protected int _maxSP;
    public int MaxSP => _maxSP;
    [SerializeField]
    protected int _SP;
    public int SP => _SP;

    [SerializeField]
    protected int _EP;
    [SerializeField]
    protected int _MaxEP;
    public int EP => _EP;
    public void SetEP(int value)
    {
        int Result = _EP + value;
        _EP = Mathf.Clamp(Result, 0, _MaxEP);
    }

    public int MaxEP => _MaxEP;

    [SerializeField]
    protected int _STR;

    [SerializeField]
    protected int _shield;

    public float StiffTime { get; set; }
    public int STR => _STR;
    [SerializeField]
    protected int _DEF;
    public int DEF => _DEF;
    [SerializeField]
    protected float _MoveSpeed;
    public float MoveSpeed => _MoveSpeed;

    protected bool _FaceRight { get; set; }= true;
    
    
    public bool FaceRight => _FaceRight;

    public int Shield
    {
        get => _shield;
        set => _shield = value;
    }

    /// <summary>
    /// 有改變方向為true，沒改變方向為false
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public bool SetFace(float move)
    {
        if (move==0f) return false;
        bool Right = move > 0;
        if (_FaceRight ^ Right)
        {
            _FaceRight = !_FaceRight;
            return true;
        }
        else return false;
    }
    public bool SetMaxHP(int MaxHP)
    {
        if (MaxHP > 0)
        {
            _maxHP = MaxHP;
            return true;
        }
        else return false;
    }

    public bool SetMaxSP(int MaxSP)
    {
        if (MaxSP > 0)
        {
            _maxSP = MaxSP;
            return true;
        }
        else return false;
    }
    public void SetHP(int value)
    {
        int Result=_HP + value;
        _HP=Mathf.Clamp(Result,0,MaxHP);
    }
    public void SetSP(int value)
    {
        int Result = _SP+ value;
        _SP = Mathf.Clamp(Result, 0, MaxSP);
    }
    public void ReSet()
    {
        _HP = MaxHP;
    }
}
