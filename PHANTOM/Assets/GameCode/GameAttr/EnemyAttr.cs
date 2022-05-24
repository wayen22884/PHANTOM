using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttr : ICharacterAttr
{
    private EnemyCharacter _enemyCharacter;

    public void SetEnemyCharacter(EnemyCharacter enemyCharacter)
    {
        _enemyCharacter = enemyCharacter;
    }
    public override void Dead()
    {
        _enemyCharacter.Dead();
    }

    protected override void InJuryedAction()
    {
        _enemyCharacter.InJuryedAction();
    }

    public override void ReSet()
    {
        _baseAttr.ReSet();
    }

}
