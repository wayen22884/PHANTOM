using System;
using System.Collections.Generic;
using UnityEngine;
public enum Target
{
    enemy,
    player
}
public class DamageData
{
    public DamageData(Target Target,ICharacter Attacker,Vector3 firepoint)
    {
        _target = Target;
        _Attacker = Attacker;
        _AttackerAttr = _Attacker.Attr;
        _FaceRight = _AttackerAttr.FaceRight;
        _startPoint = firepoint;
    }
    private Target _target;
    private ICharacter _Attacker;
    private ICharacterAttr _AttackerAttr;
    private bool _FaceRight;
    private Vector3 _startPoint;
    public Vector3 startPoint => _startPoint;
    public bool FaceRight => _FaceRight;
    public ICharacter Attacker => _Attacker;
    public List<ICharacter> EnemyList
    {
        get
        {
            List<ICharacter> result;
            switch (_target)
            {
                case Target.enemy:
                    result= AllSourcePool.GetAliveEnemyListCopy();
                    break;
                case Target.player:
                {
                    List<ICharacter> list = new List<ICharacter>();
                    list.Add(AllSourcePool.PlayerCharacter);
                    result= list;
                    break;
                }
                default:
                    Debug.LogError("target is wrong.");
                    result = new List<ICharacter>();
                    break;
            }
            return result;
        }
    }

    public int attackType;
}
