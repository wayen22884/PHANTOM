using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IAttrStrategy
{
    int GetDamageValue(IBaseAttr Attacker, IBaseAttr UnderAttacker);
}
public class AttrStrategy : IAttrStrategy
{ 
    public virtual int GetDamageValue(IBaseAttr Attacker, IBaseAttr UnderAttacker)
    {
        int Result = Attacker.STR - UnderAttacker.DEF;
        if (Result <= 0)Result = 1;
        return Result;
    }
}
