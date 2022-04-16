using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerBaseData", menuName = "CreatePlayerBaseData")]
public class PlayerBaseAttr : ICharacterBaseAttr
{
    [SerializeField]
    private float _dashMultiplicator;
    public float DashMultiplicator 
    {
        get 
        {
            return _dashMultiplicator*0.01f;
        }
    }
}

