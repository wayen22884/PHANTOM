﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttr : ICharacterAttr
{
    private PlayerCharacter _playerCharacter;
    public void SetCharacter(PlayerCharacter PlayerCharacter)
    {
        _playerCharacter = PlayerCharacter;
    }
    public float DashMove { get => (_baseAttr as PlayerBaseAttr).DashMultiplicator*MoveVelocity; }
    public float NoDamageTime { get => (_baseAttr as PlayerBaseAttr).NoDamageTime; }


    public override void Dead()
    {
        Debug.Log("playerdead");
        _playerCharacter.Dead();
    }

    protected override void InJuryedAction()
    {
        _playerCharacter.InJuryedAction();
    }

    public override void ReSet()
    {
        throw new System.NotImplementedException();
    }
}
