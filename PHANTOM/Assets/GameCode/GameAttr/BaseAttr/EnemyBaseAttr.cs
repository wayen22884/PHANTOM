using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBaseData", menuName = "CreateEnemyBaseData")]
public class EnemyBaseAttr : ICharacterBaseAttr
{
    public void SetAllValue(SaveData saveData)
    {
        _FaceRight = true;
        _MoveSpeed = saveData.MoveSpeed;
        _maxHP=_HP = saveData.MaxHP;
        _STR = saveData.STR;
        _DEF = saveData.DEF;
        _attrName = saveData.AttrName;
        _attrType = saveData.AttrType;
    }
}
