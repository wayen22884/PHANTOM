using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransformAttrStragy : AttrStrategy
{
    public override int GetDamageValue(IBaseAttr Attacker, IBaseAttr UnderAttacker)
    {
        int Result = Mathf.RoundToInt(Attacker.STR * 1.25f - UnderAttacker.DEF);
        if (Result <= 0) Result = 1;
        return Result;
    }
}
